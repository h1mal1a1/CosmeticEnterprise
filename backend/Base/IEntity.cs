namespace CosmeticEnterpriseBack.Base;

public interface IEntity<Tkey>
{
    Tkey Id { get; }
}