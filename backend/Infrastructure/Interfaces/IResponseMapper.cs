namespace CosmeticEnterpriseBack.Infrastructure.Interfaces;

public interface IResponseMapper<TEntity, out TResponse>
{
    TResponse Map(TEntity entity);
}