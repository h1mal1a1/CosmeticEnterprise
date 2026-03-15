using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Base;

public class CrudService<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>
    : ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>
    where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IEntityReader<TEntity, TKey> _reader;
    private readonly ICreateMapper<TEntity, TCreateRequest> _createMapper;
    private readonly IUpdateMapper<TEntity, TUpdateRequest> _updateMapper;
    private readonly IResponseMapper<TEntity, TResponse> _responseMapper;

    public CrudService(DbContext dbContext, IEntityReader<TEntity, TKey> reader,
        ICreateMapper<TEntity, TCreateRequest> createMapper, IUpdateMapper<TEntity, TUpdateRequest> updateMapper,
        IResponseMapper<TEntity, TResponse> responseMapper)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
        _reader = reader;
        _createMapper = createMapper;
        _updateMapper = updateMapper;
        _responseMapper = responseMapper;
    }

    public async Task<IReadOnlyCollection<TResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _reader.GetAllAsync(cancellationToken);
        return entities.Select(_responseMapper.Map).ToList();
    }

    public async Task<TResponse?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _reader.GetByIdAsync(id, cancellationToken);
        return entity is null ? default : _responseMapper.Map(entity);
    }

    public async Task<TResponse> CreateAsync(TCreateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = _createMapper.Map(request);
        await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _responseMapper.Map(entity);
    }

    public async Task<TResponse?> UpdateAsync(TKey id, TUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _reader.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return default;
        _updateMapper.Map(request, entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _responseMapper.Map(entity);
    }

    public async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _reader.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}