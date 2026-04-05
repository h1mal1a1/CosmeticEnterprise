namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Остатки на скаладах
/// </summary>
public class LeftoversInWarehouses
{
    public long Id { get; set; }
    public FinishedProducts FinishedProducts { get; set; } = null!;
    public long IdFinishedProduct { get; set; }
    public Warehouses Warehouses { get; set; } = null!;
    public long IdWarehouse { get; set; }
}