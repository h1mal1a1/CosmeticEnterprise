namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Позиции заказа
/// </summary>
public class OrderItems
{
    public long Id { get; set; }
    public Orders Order { get; set; }
    public FinishedProducts FinishedProducts { get; set; }
}