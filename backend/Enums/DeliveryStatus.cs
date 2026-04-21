namespace CosmeticEnterpriseBack.Enums;

/// <summary>
/// Статус доставки
/// </summary>
public enum DeliveryStatus
{
    Pending = 1,     // Ожидает обработки
    Preparing = 2,   // Собирается
    Shipped = 3,     // Отправлен
    Delivered = 4,   // Доставлен
    Cancelled = 5    // Отменен
}