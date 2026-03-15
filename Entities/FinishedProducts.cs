using CosmeticEnterpriseBack.Base;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Готовая продукция
/// </summary>
public class FinishedProducts : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<OrderItems> OrderItemsList { get; set; }
    public Recipes Recipe { get; set; }
    public ProductCategories ProductCategories { get; set; }
    public List<ProductParties> ProductPartiesList { get; set; } 
    public List<LeftoversInWarehouses> LeftoversInWarehousesList { get; set; }
    public UnitsOfMeasurement UnitsOfMeasurement { get; set; }
}