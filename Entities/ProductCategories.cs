namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Категории продуктов
/// </summary>
public class ProductCategories
{
    public long Id{ get; set; }
    public List<FinishedProducts> FinishedProductsList { get; set; }
}