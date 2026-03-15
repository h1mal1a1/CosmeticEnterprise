namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Статусы заказов
/// </summary>
public class OrderStatuses
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Orders> OrdersList { get; set; } = [];
}