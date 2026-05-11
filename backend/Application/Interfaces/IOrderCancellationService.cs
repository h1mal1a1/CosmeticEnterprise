using CosmeticEnterpriseBack.Application.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderCancellationService
{
    Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken);
}