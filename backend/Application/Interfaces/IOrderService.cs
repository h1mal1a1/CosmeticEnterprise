using CosmeticEnterpriseBack.Api.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request, CancellationToken cancellationToken);

    Task<PagedResult<OrderListItemResponse>> GetMyOrdersAsync(long userId, GetOrdersQuery query, CancellationToken cancellationToken);

    Task<OrderResponse> GetMyOrderByIdAsync(long userId, long orderId, CancellationToken cancellationToken);

    Task<OrderResponse> CancelMyOrderAsync(long userId, long orderId, CancellationToken cancellationToken);

    Task<PagedResult<OrderListItemResponse>> GetAllOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken);

    Task<OrderResponse> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);

    Task<OrderResponse> UpdateOrderStatusesAsync(long orderId, UpdateOrderStatusesRequest request, CancellationToken cancellationToken);
}