namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Склады
/// </summary>
public class Warehouses
{
    public long Id { get; set; }
    public List<LeftoversInWarehouses> LeftoversInWarehousesList { get; set; }
}