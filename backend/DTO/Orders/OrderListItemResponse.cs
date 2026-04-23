using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.DTO.Orders;

public class OrderListItemResponse
{
    public long Id { get; set; }
    public long IdUser { get; set; }
    public long IdUserAddress { get; set; }
    public long IdSalesChannel { get; set; }

    public OrderStatus OrderStatus { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; }
    public PaymentType PaymentType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal DeliveryPrice { get; set; }
    public int TotalItemsQuantity { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}