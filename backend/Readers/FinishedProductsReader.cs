using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Readers;

public class FinishedProductsReader : IEntityReader<FinishedProducts, long>
{
    private readonly DbContext _dbContext;

    public FinishedProductsReader(DbContext dbContext) => _dbContext = dbContext;

    public async Task<FinishedProducts?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<FinishedProducts>()
            .Include(x => x.Images)
            .Include(x => x.LeftoversInWarehousesList)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<FinishedProducts?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<FinishedProducts>()
            .Include(x => x.Images)
            .Include(x => x.LeftoversInWarehousesList)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<FinishedProducts>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<FinishedProducts>()
            .Include(x => x.Images)
            .Include(x => x.LeftoversInWarehousesList)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}