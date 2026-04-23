using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.DTO.Orders;

public class OrderResponse
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
    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public List<OrderItemResponse> Items { get; set; } = [];
}