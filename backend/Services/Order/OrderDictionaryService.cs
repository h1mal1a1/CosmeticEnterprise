using CosmeticEnterpriseBack.DTO.Common;
using CosmeticEnterpriseBack.DTO.Orders;
using CosmeticEnterpriseBack.Enums;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Services.Order;

public class OrderDictionaryService : IOrderDictionaryService
{
    public Task<OrderDictionariesResponse> GetOrderDictionariesAsync(CancellationToken cancellationToken)
    {
        var response = new OrderDictionariesResponse
        {
            OrderStatuses = GetEnumOptions<OrderStatus>(GetOrderStatusDisplayName),
            DeliveryStatuses = GetEnumOptions<DeliveryStatus>(GetDeliveryStatusDisplayName),
            PaymentTypes = GetEnumOptions<PaymentType>(GetPaymentTypeDisplayName),
            PaymentMethods = GetEnumOptions<PaymentMethod>(GetPaymentMethodDisplayName),
            PaymentStatuses = GetEnumOptions<PaymentStatus>(GetPaymentStatusDisplayName)
        };

        return Task.FromResult(response);
    }

    private static IReadOnlyCollection<EnumOptionResponse> GetEnumOptions<TEnum>(Func<TEnum, string> displayNameFactory)
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(x => new EnumOptionResponse
            {
                Value = Convert.ToInt32(x),
                Name = x.ToString(),
                DisplayName = displayNameFactory(x)
            })
            .ToList();
    }

    private static string GetOrderStatusDisplayName(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Created => "Создан",
            OrderStatus.AwaitingPayment => "Ожидает оплаты",
            OrderStatus.Paid => "Оплачен",
            OrderStatus.Processing => "В обработке",
            OrderStatus.Completed => "Завершен",
            OrderStatus.Cancelled => "Отменен",
            _ => status.ToString()
        };
    }

    private static string GetDeliveryStatusDisplayName(DeliveryStatus status)
    {
        return status switch
        {
            DeliveryStatus.Pending => "Ожидает обработки",
            DeliveryStatus.Preparing => "Собирается",
            DeliveryStatus.Shipped => "Отправлен",
            DeliveryStatus.Delivered => "Доставлен",
            DeliveryStatus.Cancelled => "Отменен",
            _ => status.ToString()
        };
    }

    private static string GetPaymentTypeDisplayName(PaymentType type)
    {
        return type switch
        {
            PaymentType.Immediate => "Оплата сразу",
            PaymentType.Postpaid => "Постоплата",
            _ => type.ToString()
        };
    }

    private static string GetPaymentMethodDisplayName(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Cash => "Наличными",
            PaymentMethod.BankTransfer => "Банковский перевод",
            PaymentMethod.Sbp => "СБП",
            _ => method.ToString()
        };
    }

    private static string GetPaymentStatusDisplayName(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => "Ожидает оплаты",
            PaymentStatus.Paid => "Оплачен",
            PaymentStatus.Failed => "Ошибка оплаты",
            _ => status.ToString()
        };
    }
}