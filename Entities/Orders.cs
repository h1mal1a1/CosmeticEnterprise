namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Заказы
/// </summary>
public class Orders
{
    public long Id { get; set; }
    public long IdCustomers { get; set; }
    public long IdSalesChannels { get; set; }
    public long IdOrderStatuses{ get; set; }
    public Customers? Customer { get; set; }
    public SalesChannels? SalesChannel{ get; set; }
    public OrderStatuses? OrderStatus{ get; set; }
    public List<OrderItems>? LstOrderItems { get; set; }
}