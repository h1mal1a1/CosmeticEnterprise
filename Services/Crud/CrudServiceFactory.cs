using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.Crud;

public class CrudServiceFactory: ICrudServiceFactory
{
    private readonly DbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAuthorizationService _authorizationService;


    public CrudServiceFactory(DbContext dbContext, IServiceProvider serviceProvider,
        IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _authorizationService = authorizationService;
    }

    public ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>
        Create<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(ResourceType type)
        where TEntity : class
    {
        var reader = _serviceProvider.GetRequiredService<IEntityReader<TEntity, TKey>>();
        var createMapper = _serviceProvider.GetRequiredService<ICreateMapper<TEntity, TCreateRequest>>();
        var updateMapper = _serviceProvider.GetRequiredService<IUpdateMapper<TEntity, TUpdateRequest>>();
        var responseMapper = _serviceProvider.GetRequiredService<IResponseMapper<TEntity, TResponse>>();

        return new CrudService<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(_dbContext, reader,
            createMapper, updateMapper, responseMapper,
            _authorizationService, type);
    }
}