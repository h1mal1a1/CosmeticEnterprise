namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Поставщики
/// </summary>
public class Suppliers
{
    public long Id { get; set; }
    public List<SuppliesFromSuppliers> SuppliesFromSuppliersList { get; set; } = [];
}