namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Остатки на складах
/// </summary>
public class LeftoversInWarehouses
{
    public long Id { get; set; }

    public long IdFinishedProduct { get; set; }
    public FinishedProducts FinishedProducts { get; set; } = null!;

    public long IdWarehouse { get; set; }
    public Warehouses Warehouses { get; set; } = null!;

    /// <summary>
    /// Фактический остаток
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Зарезервированное количество
    /// </summary>
    public int ReservedQuantity { get; set; }
}