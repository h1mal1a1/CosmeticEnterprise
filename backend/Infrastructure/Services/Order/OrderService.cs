using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.DTOs.Orders;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Order;

public class OrderService(IOrderCreationService orderCreationService, IOrderReadService orderReadService,
    IOrderCancellationService orderCancellationService, IOrderStatusUpdateService orderStatusUpdateService) 
    : IOrderService
{
    public Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request, 
        CancellationToken cancellationToken) =>  orderCreationService.CreateOrderFromCartAsync(
            userId, request, cancellationToken);

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