namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Клиенты
/// </summary>
public class Customers
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Orders>? LstOrders { get; set; }
}