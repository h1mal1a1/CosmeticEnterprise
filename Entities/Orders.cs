namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Заказы
/// </summary>
public class Orders
{
    public long Id { get; set; }

    public Customers Customer { get; set; } = null!;
    public long IdCustomer { get; set; }
    public SalesChannels SalesChannel { get; set; } = null!;
    public long IdSalesChannel { get; set; }
    public OrderStatuses OrderStatus { get; set; } = null!;
    public long IdOrderStatus { get; set; }
    public List<OrderItems> OrderItemsList { get; set; } = [];
}