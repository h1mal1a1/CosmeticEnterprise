using CosmeticEnterpriseBack.Application.Models.Orders;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IOrderStockService
{
    Task<OrderStockReservationResult> ReserveCartItemsAsync(IReadOnlyCollection<ShoppingCartItem> cartItems,
        CancellationToken cancellationToken);

    Task ReleaseReserveAsync(Orders order, CancellationToken cancellationToken);

    Task ConsumeReservedStockAsync(Orders order, CancellationToken cancellationToken);
}