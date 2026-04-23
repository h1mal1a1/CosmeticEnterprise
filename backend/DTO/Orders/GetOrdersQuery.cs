using CosmeticEnterpriseBack.Enums;

namespace CosmeticEnterpriseBack.DTO.Orders;

public class GetOrdersQuery
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public OrderStatus? OrderStatus { get; set; }

    public DeliveryStatus? DeliveryStatus { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    public long? IdUser { get; set; }
}