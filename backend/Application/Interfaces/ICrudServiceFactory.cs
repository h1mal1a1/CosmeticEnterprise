using CosmeticEnterpriseBack.Application.Authorization;
namespace CosmeticEnterpriseBack.Application.Interfaces;
public interface ICrudServiceFactory
{
    ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>
        Create<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(ResourceType resourceType)
        where TEntity : class;
}