using CosmeticEnterpriseBack.Authorization;

namespace CosmeticEnterpriseBack.Interfaces;

public interface ICrudServiceFactory
{
    ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>
        Create<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(ResourceType resourceType)
        where TEntity : class;
}