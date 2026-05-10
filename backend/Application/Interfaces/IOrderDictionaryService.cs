using CosmeticEnterpriseBack.Api.DTOs.Orders;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderDictionaryService
{
    Task<OrderDictionariesResponse> GetOrderDictionariesAsync(CancellationToken cancellationToken);
}