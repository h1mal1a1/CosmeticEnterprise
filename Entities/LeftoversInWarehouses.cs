namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Остатки на скаладах
/// </summary>
public class LeftoversInWarehouses
{
    public long Id { get; set; }
    public FinishedProducts FinishedProducts { get; set; }
    public Warehouses Warehouses { get; set; }
}