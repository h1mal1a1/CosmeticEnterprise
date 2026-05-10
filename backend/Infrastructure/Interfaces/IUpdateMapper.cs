namespace CosmeticEnterpriseBack.Infrastructure.Interfaces;
public interface IUpdateMapper<TEntity, in TUpdateRequest>
{
    void Map(TUpdateRequest request, TEntity entity);
}