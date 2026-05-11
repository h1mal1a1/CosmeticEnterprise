using CosmeticEnterpriseBack.Application.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderStatusUpdateService
{
    Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request, CancellationToken cancellationToken);
}