using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Interfaces.Persistence;

public interface IFinishedProductStockRepository
{
    Task<bool> FinishedProductExistsAsync(long id, CancellationToken cancellationToken);

    Task<List<LeftoversInWarehouses>> GetLeftoversAsync(long finishedProductId, CancellationToken cancellationToken);

    Task<Warehouses> GetOrCreateWarehouseAsync(CancellationToken cancellationToken);

    void AddLeftover(LeftoversInWarehouses leftover);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}