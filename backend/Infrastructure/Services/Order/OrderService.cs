using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Validators;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderService(AppDbContext dbContext, IOrderStockService orderStockService, 
    IOrderReturnUrlValidator orderReturnUrlValidator, IOrderReadService orderReadService, 
    IOrderStatusUpdateService orderStatusUpdateService, IOrderCancellationService orderCancellationService) 
    : IOrderService
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
            var reservation = await orderStockService.ReserveCartItemsAsync([.. cart.Items], cancellationToken);

            var orderStatus = OrderStatus.Created;

            var order = new Orders
            {
                IdUser = userId,
                IdUserAddress = request.IdUserAddress,
                IdSalesChannel = salesChannel.Id,
                OrderStatus = orderStatus,
                DeliveryStatus = DeliveryStatus.Pending,
                PaymentType = request.PaymentType,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = reservation.TotalAmount,
                DeliveryPrice = 0m,
                Comment = request.Comment,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                OrderItemsList = [.. reservation.OrderItems]
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

    public Task<PagedResult<OrderListItemResponse>> GetMyOrdersAsync(long userId, GetOrdersQuery query,
        CancellationToken cancellationToken) => orderReadService.GetMyOrdersAsync(userId, query, cancellationToken);
    public Task<OrderResponse> GetMyOrderByIdAsync(long userId, long orderId, CancellationToken cancellationToken) => 
        orderReadService.GetMyOrderByIdAsync(userId, orderId, cancellationToken);

    public Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken) => 
        orderCancellationService.CancelMyOrderAsync(userId, orderId, cancellationToken);

    public Task<PagedResult<OrderListItemResponse>> GetAllOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken) =>
        orderReadService.GetAllOrdersAsync(query, cancellationToken);

    public Task<OrderResponse> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken) => 
        orderReadService.GetOrderByIdAsync(orderId, cancellationToken);

    public Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request, 
        CancellationToken cancellationToken) => orderStatusUpdateService.UpdateOrderStatusesAsync(
            orderId, request, cancellationToken);
}