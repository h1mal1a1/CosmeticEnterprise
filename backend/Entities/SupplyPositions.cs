namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Позиции поставки
/// </summary>
public class SupplyPositions
{
    public long Id { get; set; }
    public SuppliesFromSuppliers SuppliesFromSuppliers { get; set; } = null!;
    public long IdSuppliesFromSupplier { get; set; }
    public Materials Material { get; set; } = null!;
    public long IdMaterial { get; set; }
}