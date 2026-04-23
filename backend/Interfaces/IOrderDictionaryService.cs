using CosmeticEnterpriseBack.DTO.Orders;

namespace CosmeticEnterpriseBack.Interfaces;

public interface IOrderDictionaryService
{
    Task<OrderDictionariesResponse> GetOrderDictionariesAsync(CancellationToken cancellationToken);
}