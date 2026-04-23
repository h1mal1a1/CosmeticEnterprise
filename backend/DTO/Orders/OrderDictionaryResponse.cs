using CosmeticEnterpriseBack.DTO.Common;

namespace CosmeticEnterpriseBack.DTO.Orders;

public class OrderDictionariesResponse
{
    public IReadOnlyCollection<EnumOptionResponse> OrderStatuses { get; set; } = [];

    public IReadOnlyCollection<EnumOptionResponse> DeliveryStatuses { get; set; } = [];

    public IReadOnlyCollection<EnumOptionResponse> PaymentTypes { get; set; } = [];

    public IReadOnlyCollection<EnumOptionResponse> PaymentMethods { get; set; } = [];

    public IReadOnlyCollection<EnumOptionResponse> PaymentStatuses { get; set; } = [];
}