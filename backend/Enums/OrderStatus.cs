namespace CosmeticEnterpriseBack.Enums;

/// <summary>
/// Статус заказа
/// </summary>
public enum OrderStatus
{
    Created = 1,      // Создан
    Processing = 2,   // В обработке
    Completed = 3,    // Завершен
    Cancelled = 4     // Отменен
}