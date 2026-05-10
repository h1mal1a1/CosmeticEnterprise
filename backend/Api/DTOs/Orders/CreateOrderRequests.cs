using CosmeticEnterpriseBack.Domain.Enums;
namespace CosmeticEnterpriseBack.Api.DTOs.Orders;
public class CreateOrderRequest
{
    public long IdUserAddress { get; set; }
    public PaymentType PaymentType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? ReturnUrl { get; set; }
}