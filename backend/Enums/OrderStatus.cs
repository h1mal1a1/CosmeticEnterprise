namespace CosmeticEnterpriseBack.Enums;

/// <summary>
/// Статус заказа
/// </summary>
public enum OrderStatus
{
    Created = 1,           // Создан
    AwaitingPayment = 2,   // Ожидает оплаты
    Paid = 3,              // Оплачен
    Processing = 4,        // В обработке
    Completed = 5,         // Завершен
    Cancelled = 6          // Отменен
}