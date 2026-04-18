using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Readers;

public class EntityReader<TEntity, TKey> : IEntityReader<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly DbContext _dbContext;
    public EntityReader(DbContext dbContext) => _dbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
    }

    public async Task<TEntity?> GetByIdForUpdateAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id != null && x.Id.Equals(id), cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}