namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Каналы продаж
/// </summary>
public class SalesChannels
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Orders> OrdersList { get; set; } = [];
}