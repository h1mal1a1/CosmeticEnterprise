using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderCancellationService(AppDbContext dbContext, IOrderStockService orderStockService,
    IOrderMapper orderMapper) : IOrderCancellationService
{
    public async Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(
                x => x.Id == orderId && x.IdUser == userId,
                cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.OrderStatus == OrderStatus.Cancelled)
            return orderMapper.ToResponse(order);

        if (order.OrderStatus == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be cancelled.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await orderStockService.ReleaseReserveAsync(order, cancellationToken);

            order.OrderStatus = OrderStatus.Cancelled;
            order.DeliveryStatus = DeliveryStatus.Cancelled;
            order.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return orderMapper.ToResponse(order);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}