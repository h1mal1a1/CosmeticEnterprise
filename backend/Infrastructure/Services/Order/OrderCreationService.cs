using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Validators;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderCreationService(AppDbContext dbContext, IOrderStockService orderStockService,
    IOrderReturnUrlValidator orderReturnUrlValidator, IOrderReadService orderReadService) 
    : IOrderCreationService
{
    private const string WebsiteSalesChannelName = "Website";

    public async Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        orderReturnUrlValidator.Validate(request.ReturnUrl);

        var userAddress = await dbContext.UserAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.IdUserAddress && x.IdUser == userId,
                cancellationToken);

        if (userAddress is null)
            throw new KeyNotFoundException("User address not found.");

        var salesChannel = await dbContext.SalesChannels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == WebsiteSalesChannelName, cancellationToken);

        if (salesChannel is null)
            throw new KeyNotFoundException("Website sales channel not found.");

        var cart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.IdUser == userId, cancellationToken);

        if (cart is null || cart.Items.Count == 0)
            throw new InvalidOperationException("Shopping cart is empty.");

        var now = DateTime.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var reservation = await orderStockService.ReserveCartItemsAsync(
                cart.Items.ToList(),
                cancellationToken);

            var order = new Orders
            {
                IdUser = userId,
                IdUserAddress = request.IdUserAddress,
                IdSalesChannel = salesChannel.Id,
                OrderStatus = OrderStatus.Created,
                DeliveryStatus = DeliveryStatus.Pending,
                PaymentType = request.PaymentType,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = reservation.TotalAmount,
                DeliveryPrice = 0m,
                Comment = request.Comment,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                OrderItemsList = reservation.OrderItems.ToList()
            };

            dbContext.Orders.Add(order);

            dbContext.ShoppingCartItems.RemoveRange(cart.Items);
            cart.UpdatedAtUtc = now;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await orderReadService.GetMyOrderByIdAsync(userId, order.Id, cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}