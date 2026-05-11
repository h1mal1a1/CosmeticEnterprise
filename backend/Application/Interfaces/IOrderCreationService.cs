using CosmeticEnterpriseBack.Application.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderCreationService
{
    Task<OrderResponse> CreateOrderFromCartAsync(long userId, CreateOrderRequest request, CancellationToken cancellationToken);
}