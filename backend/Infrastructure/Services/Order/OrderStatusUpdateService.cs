using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Application.Validators;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderStatusUpdateService(AppDbContext dbContext, IOrderStockService orderStockService,
    IOrderStatusTransitionValidator orderStatusTransitionValidator, IOrderMapper orderMapper) : IOrderStatusUpdateService
{
    public async Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request,
        CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.OrderItemsList)
                .ThenInclude(x => x.FinishedProducts)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        orderStatusTransitionValidator.Validate(order, request);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var becomesCancelled =
                order.OrderStatus != OrderStatus.Cancelled &&
                request.OrderStatus == OrderStatus.Cancelled;

            var becomesCompleted =
                order.OrderStatus != OrderStatus.Completed &&
                request.OrderStatus == OrderStatus.Completed;

            if (becomesCancelled)
                await orderStockService.ReleaseReserveAsync(order, cancellationToken);

            if (becomesCompleted)
                await orderStockService.ConsumeReservedStockAsync(order, cancellationToken);

            order.OrderStatus = request.OrderStatus;
            order.DeliveryStatus = request.DeliveryStatus;
            order.PaymentStatus = request.PaymentStatus;
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