using CosmeticEnterpriseBack.Application.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderReadService
{
    Task<PagedResult<OrderListItemResponse>> GetMyOrdersAsync(long userId, GetOrdersQuery query, CancellationToken cancellationToken);

    Task<OrderResponse> GetMyOrderByIdAsync(long userId, long orderId, CancellationToken cancellationToken);

    Task<PagedResult<OrderListItemResponse>> GetAllOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken);

    Task<OrderResponse> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);
}