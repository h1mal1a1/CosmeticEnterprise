namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Каналы продаж
/// </summary>
public class SalesChannels
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Orders> LstOrders { get; set; }
}