using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.DTO.Orders;

/// <summary>
/// Запрос на создание заказа (checkout)
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Адрес доставки
    /// </summary>
    public long IdUserAddress { get; set; }

    /// <summary>
    /// Тип оплаты
    /// </summary>
    public PaymentType PaymentType { get; set; }

    /// <summary>
    /// Способ оплаты
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Комментарий к заказу
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// URL возврата после оплаты
    /// </summary>
    public string? ReturnUrl { get; set; }
}