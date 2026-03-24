using CosmeticEnterpriseBack.Base;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Категории продуктов
/// </summary>
public class ProductCategories : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<FinishedProducts> FinishedProductsList { get; set; } = [];
}