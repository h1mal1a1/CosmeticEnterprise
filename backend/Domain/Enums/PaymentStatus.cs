namespace CosmeticEnterpriseBack.Domain.Enums;

/// <summary>
/// Статус оплаты
/// </summary>
public enum PaymentStatus
{
    Pending = 1,   // Ожидает оплаты
    Paid = 2,      // Оплачен
    Failed = 3     // Ошибка оплаты
}