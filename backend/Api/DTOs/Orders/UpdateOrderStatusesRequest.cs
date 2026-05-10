using CosmeticEnterpriseBack.Domain.Enums;
namespace CosmeticEnterpriseBack.Api.DTOs.Orders;
public class UpdateOrderStatusesRequest
{
    public OrderStatus OrderStatus { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}