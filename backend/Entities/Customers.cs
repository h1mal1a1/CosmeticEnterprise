namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Клиенты
/// </summary>
public class Customers
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Orders> OrdersList { get; set; } = [];
}