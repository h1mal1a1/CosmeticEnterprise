using CosmeticEnterpriseBack.Base;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Готовая продукция
/// </summary>
public class FinishedProducts : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<OrderItems> OrderItemsList { get; set; } = [];
    public long IdRecipe { get; set; }
    public Recipes Recipe { get; set; } = null!;
    public long IdProductCategory { get; set; }
    public ProductCategories ProductCategories { get; set; } = null!;
    public List<ProductParties> ProductPartiesList { get; set; } = [];
    public List<LeftoversInWarehouses> LeftoversInWarehousesList { get; set; } = [];
    public long IdUnitsOfMeasurement { get; set; }
    public UnitsOfMeasurement UnitsOfMeasurement { get; set; } = null!;
    public ICollection<FinishedProductImages> Images { get; set; } = [];
}