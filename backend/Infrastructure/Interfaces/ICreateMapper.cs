namespace CosmeticEnterpriseBack.Infrastructure.Interfaces;
public interface ICreateMapper<TEntity, in TCreateRequest>
{
    TEntity Map(TCreateRequest request);
}