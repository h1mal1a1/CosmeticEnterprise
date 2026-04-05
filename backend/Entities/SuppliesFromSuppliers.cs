namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Поставки от поставщиков
/// </summary>
public class SuppliesFromSuppliers
{
    public long Id { get; set; }
    public Suppliers Supplier { get; set; } = null!;
    public long IdSupplier { get; set; }
    public List<SupplyPositions> SupplyPositionsList { get; set; } = [];
}