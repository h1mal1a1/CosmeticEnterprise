using System.ComponentModel.DataAnnotations;
namespace CosmeticEnterpriseBack.Api.DTOs.Cart;
public class UpdateCartItemQuantityRequest
{
    [Range(1, 999)]
    public int Quantity { get; set; }
}