using System.ComponentModel.DataAnnotations;
namespace CosmeticEnterpriseBack.Application.DTOs.Cart;
public class UpdateCartItemQuantityRequest
{
    [Range(1, 999)]
    public int Quantity { get; set; }
}