using CosmeticEnterpriseBack.Application.DTOs.Orders;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Enums;

namespace CosmeticEnterpriseBack.Application.Validators;

public class OrderStatusTransitionValidator : IOrderStatusTransitionValidator
{
    public void Validate(Orders order, UpdateOrderStatusesRequest request)
    {
        if (order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Completed)
        {
            throw new InvalidOperationException(
                "Cancelled or completed order cannot be changed.");
        }

        if (request.PaymentStatus == PaymentStatus.Paid &&
            request.OrderStatus == OrderStatus.Created)
        {
            throw new InvalidOperationException(
                "Paid order cannot remain in Created status.");
        }

        if (request.DeliveryStatus == DeliveryStatus.Delivered &&
            request.OrderStatus != OrderStatus.Completed)
        {
            throw new InvalidOperationException(
                "Delivered order must have Completed order status.");
        }

        if (request.OrderStatus == OrderStatus.Cancelled &&
            request.DeliveryStatus != DeliveryStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Cancelled order must have Cancelled delivery status.");
        }

        if (request.OrderStatus == OrderStatus.Completed &&
            request.PaymentStatus != PaymentStatus.Paid)
        {
            throw new InvalidOperationException(
                "Completed order must have Paid payment status.");
        }
    }
}