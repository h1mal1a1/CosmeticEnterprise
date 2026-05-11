using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Models.Orders;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderStockService(AppDbContext dbContext) : IOrderStockService
{
    public async Task ReleaseReserveAsync(Orders order, CancellationToken cancellationToken)
    {
        var orderProductIds = order.OrderItemsList
            .Select(x => x.IdFinishedProduct)
            .Distinct()
            .ToList();

        var leftovers = await dbContext.LeftoversInWarehouses
            .Where(x => orderProductIds.Contains(x.IdFinishedProduct))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var orderItem in order.OrderItemsList)
        {
            var quantityToRelease = orderItem.Quantity;

            var productLeftovers = leftovers
                .Where(x => x.IdFinishedProduct == orderItem.IdFinishedProduct)
                .OrderByDescending(x => x.Id)
                .ToList();

            var reservedTotal = productLeftovers.Sum(x => x.ReservedQuantity);

            if (reservedTotal < quantityToRelease)
            {
                throw new InvalidOperationException(
                    $"Cannot release reserve for product id {orderItem.IdFinishedProduct}. Reserved: {reservedTotal}, expected: {quantityToRelease}.");
            }

            foreach (var leftover in productLeftovers)
            {
                if (leftover.ReservedQuantity <= 0)
                    continue;

                var releaseFromCurrent = Math.Min(leftover.ReservedQuantity, quantityToRelease);
                leftover.ReservedQuantity -= releaseFromCurrent;
                quantityToRelease -= releaseFromCurrent;

                if (quantityToRelease == 0)
                    break;
            }
        }
    }

    public async Task ConsumeReservedStockAsync(Orders order, CancellationToken cancellationToken)
    {
        var orderProductIds = order.OrderItemsList
            .Select(x => x.IdFinishedProduct)
            .Distinct()
            .ToList();

        var leftovers = await dbContext.LeftoversInWarehouses
            .Where(x => orderProductIds.Contains(x.IdFinishedProduct))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var orderItem in order.OrderItemsList)
        {
            var quantityToConsume = orderItem.Quantity;

            var productLeftovers = leftovers
                .Where(x => x.IdFinishedProduct == orderItem.IdFinishedProduct)
                .OrderBy(x => x.Id)
                .ToList();

            var reservedTotal = productLeftovers.Sum(x => x.ReservedQuantity);

            if (reservedTotal < quantityToConsume)
            {
                throw new InvalidOperationException(
                    $"Cannot complete order for product id {orderItem.IdFinishedProduct}. Reserved: {reservedTotal}, expected: {quantityToConsume}.");
            }

            foreach (var leftover in productLeftovers)
            {
                if (leftover.ReservedQuantity <= 0 || leftover.Quantity <= 0)
                    continue;

                var consumeFromCurrent = Math.Min(
                    Math.Min(leftover.ReservedQuantity, leftover.Quantity),
                    quantityToConsume);

                leftover.ReservedQuantity -= consumeFromCurrent;
                leftover.Quantity -= consumeFromCurrent;
                quantityToConsume -= consumeFromCurrent;

                if (quantityToConsume == 0)
                    break;
            }

            if (quantityToConsume > 0)
            {
                throw new InvalidOperationException(
                    $"Unable to fully consume reserved stock for product id {orderItem.IdFinishedProduct}.");
            }
        }
    }

    public async Task<OrderStockReservationResult> ReserveCartItemsAsync(
    IReadOnlyCollection<ShoppingCartItem> cartItems,
    CancellationToken cancellationToken)
{
    var cartItemProductIds = cartItems
        .Select(x => x.IdFinishedProduct)
        .Distinct()
        .ToList();

    var products = await dbContext.FinishedProducts
        .Where(x => cartItemProductIds.Contains(x.Id))
        .ToDictionaryAsync(x => x.Id, cancellationToken);

    var leftovers = await dbContext.LeftoversInWarehouses
        .Where(x => cartItemProductIds.Contains(x.IdFinishedProduct))
        .ToListAsync(cancellationToken);

    decimal totalAmount = 0m;
    var orderItems = new List<OrderItems>();

    foreach (var cartItem in cartItems)
    {
        if (!products.TryGetValue(cartItem.IdFinishedProduct, out var product))
            throw new KeyNotFoundException(
                $"Finished product with id {cartItem.IdFinishedProduct} not found.");

        var productLeftovers = leftovers
            .Where(x => x.IdFinishedProduct == cartItem.IdFinishedProduct)
            .OrderBy(x => x.Id)
            .ToList();

        var availableQuantity = productLeftovers.Sum(x => x.Quantity - x.ReservedQuantity);

        if (availableQuantity < cartItem.Quantity)
        {
            throw new InvalidOperationException(
                $"Not enough stock for product '{product.Name}'. Available: {availableQuantity}, requested: {cartItem.Quantity}.");
        }

        var quantityToReserve = cartItem.Quantity;

        foreach (var leftover in productLeftovers)
        {
            var freeQuantity = leftover.Quantity - leftover.ReservedQuantity;

            if (freeQuantity <= 0)
                continue;

            var reserveFromCurrent = Math.Min(freeQuantity, quantityToReserve);
            leftover.ReservedQuantity += reserveFromCurrent;
            quantityToReserve -= reserveFromCurrent;

            if (quantityToReserve == 0)
                break;
        }

        var unitPrice = product.Price;
        var lineTotal = unitPrice * cartItem.Quantity;

        totalAmount += lineTotal;

        orderItems.Add(new OrderItems
        {
            IdFinishedProduct = cartItem.IdFinishedProduct,
            Quantity = cartItem.Quantity,
            UnitPrice = unitPrice,
            LineTotal = lineTotal
        });
    }

    return new OrderStockReservationResult
    {
        TotalAmount = totalAmount,
        OrderItems = orderItems
    };
}
}