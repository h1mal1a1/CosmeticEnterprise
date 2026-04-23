using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.DTO.Orders;

/// <summary>
/// Обновление статусов заказа менеджером / администратором
/// </summary>
public class UpdateOrderStatusesRequest
{
    public OrderStatus OrderStatus { get; set; }

    public DeliveryStatus DeliveryStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
}