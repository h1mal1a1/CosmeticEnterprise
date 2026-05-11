using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderStockService
{
    Task ReleaseReserveAsync(Orders order, CancellationToken cancellationToken);

    Task ConsumeReservedStockAsync(Orders order, CancellationToken cancellationToken);
}