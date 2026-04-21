namespace CosmeticEnterpriseBack.Enums;

/// <summary>
/// Способ оплаты
/// </summary>
public enum PaymentMethod
{
    Cash = 1,          // Наличными
    BankTransfer = 2,  // Перевод
    Sbp = 3            // СБП
}