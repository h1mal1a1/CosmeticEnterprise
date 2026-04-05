namespace CosmeticEnterpriseBack.Interfaces;

public interface IUpdateMapper<TEntity, in TUpdateRequest>
{
    void Map(TUpdateRequest request, TEntity entity);
}