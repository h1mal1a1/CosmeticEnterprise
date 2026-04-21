using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Заказы
/// </summary>
public class Orders
{
    public long Id { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public long IdUser { get; set; }
    public User User { get; set; } = null!;

    /// <summary>
    /// Канал продаж
    /// </summary>
    public long IdSalesChannel { get; set; }
    public SalesChannels SalesChannel { get; set; } = null!;

    /// <summary>
    /// Статус заказа
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// Статус доставки
    /// </summary>
    public DeliveryStatus DeliveryStatus { get; set; }

    /// <summary>
    /// Тип оплаты
    /// </summary>
    public PaymentType PaymentType { get; set; }

    /// <summary>
    /// Способ оплаты
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Статус оплаты
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Общая сумма заказа
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Стоимость доставки
    /// </summary>
    public decimal DeliveryPrice { get; set; }

    /// <summary>
    /// Комментарий к заказу
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Дата создания заказа
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Дата обновления заказа
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    public List<OrderItems> OrderItemsList { get; set; } = [];
}