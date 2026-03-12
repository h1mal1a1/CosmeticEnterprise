namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Статусы заказов
/// </summary>
public class OrderStatuses
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Orders> LstOrders { get; set; }
}