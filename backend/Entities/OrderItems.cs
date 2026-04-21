namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Позиции заказа
/// </summary>
public class OrderItems
{
    public long Id { get; set; }

    public long IdOrder { get; set; }
    public Orders Order { get; set; } = null!;

    public long IdFinishedProduct { get; set; }
    public FinishedProducts FinishedProducts { get; set; } = null!;

    /// <summary>
    /// Количество товара
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара на момент заказа
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Сумма по позиции
    /// </summary>
    public decimal LineTotal { get; set; }
}