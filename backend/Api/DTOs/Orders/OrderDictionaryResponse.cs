using CosmeticEnterpriseBack.Api.DTOs.Common;
namespace CosmeticEnterpriseBack.Api.DTOs.Orders;
public class OrderDictionariesResponse
{
    public IReadOnlyCollection<EnumOptionResponse> OrderStatuses { get; set; } = [];
    public IReadOnlyCollection<EnumOptionResponse> DeliveryStatuses { get; set; } = [];
    public IReadOnlyCollection<EnumOptionResponse> PaymentTypes { get; set; } = [];
    public IReadOnlyCollection<EnumOptionResponse> PaymentMethods { get; set; } = [];
    public IReadOnlyCollection<EnumOptionResponse> PaymentStatuses { get; set; } = [];
}