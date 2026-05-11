using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Models.Orders;

public class OrderStockReservationResult
{
    public decimal TotalAmount { get; init; }

    public IReadOnlyCollection<OrderItems> OrderItems { get; init; } = [];
}