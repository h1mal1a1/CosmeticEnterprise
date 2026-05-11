using CosmeticEnterpriseBack.Application.Interfaces.Persistence;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Persistence.Repositories;

public class FinishedProductStockRepository(AppDbContext dbContext)
    : IFinishedProductStockRepository
{
    public async Task<bool> FinishedProductExistsAsync(long id, CancellationToken cancellationToken) => 
        await dbContext.FinishedProducts.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task<List<LeftoversInWarehouses>> GetLeftoversAsync(long finishedProductId, CancellationToken cancellationToken) => 
        await dbContext.LeftoversInWarehouses
            .Where(x => x.IdFinishedProduct == finishedProductId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Warehouses> GetOrCreateWarehouseAsync(CancellationToken cancellationToken)
    {
        var warehouse = await dbContext.Warehouses
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (warehouse is not null)
            return warehouse;

        warehouse = new Warehouses();

        dbContext.Warehouses.Add(warehouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        return warehouse;
    }

    public void AddLeftover(LeftoversInWarehouses leftover)
    {
        dbContext.LeftoversInWarehouses.Add(leftover);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}