using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

public class OrderMapper : IOrderMapper
{
    public OrderListItemResponse ToListItemResponse(Orders order)
    {
        return new OrderListItemResponse
        {
            Id = order.Id,
            IdUser = order.IdUser,
            Username = order.User.Username,
            IdUserAddress = order.IdUserAddress,
            IdSalesChannel = order.IdSalesChannel,
            OrderStatus = order.OrderStatus,
            DeliveryStatus = order.DeliveryStatus,
            PaymentType = order.PaymentType,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount,
            DeliveryPrice = order.DeliveryPrice,
            TotalItemsQuantity = order.OrderItemsList.Sum(x => x.Quantity),
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc
        };
    }

    public OrderResponse ToResponse(Orders order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            IdUser = order.IdUser,
            Username = order.User.Username,
            IdUserAddress = order.IdUserAddress,
            IdSalesChannel = order.IdSalesChannel,
            DeliveryAddress = FormatAddress(order.UserAddress),
            OrderStatus = order.OrderStatus,
            DeliveryStatus = order.DeliveryStatus,
            PaymentType = order.PaymentType,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount,
            DeliveryPrice = order.DeliveryPrice,
            Comment = order.Comment,
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc,
            Items = order.OrderItemsList
                .OrderBy(x => x.Id)
                .Select(x => new OrderItemResponse
                {
                    Id = x.Id,
                    IdFinishedProduct = x.IdFinishedProduct,
                    ProductName = x.FinishedProducts.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    LineTotal = x.LineTotal
                })
                .ToList()
        };
    }

    private static string FormatAddress(UserAddress address)
    {
        var parts = new List<string>
        {
            address.Country,
            address.City,
            address.Street,
            address.House
        };

        if (!string.IsNullOrWhiteSpace(address.Apartment))
            parts.Add($"кв./офис {address.Apartment}");

        if (!string.IsNullOrWhiteSpace(address.PostalCode))
            parts.Add($"индекс {address.PostalCode}");

        return string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}