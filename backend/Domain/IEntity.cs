namespace CosmeticEnterpriseBack.Domain;

public interface IEntity<Tkey>
{
    Tkey Id { get; }
}