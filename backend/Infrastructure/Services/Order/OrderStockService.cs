using CosmeticEnterpriseBack.Application.Interfaces;
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
}