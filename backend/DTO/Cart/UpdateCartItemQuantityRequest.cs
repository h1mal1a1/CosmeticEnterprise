using System.ComponentModel.DataAnnotations;

namespace CosmeticEnterpriseBack.DTO.Cart;

public class UpdateCartItemQuantityRequest
{
    [Range(1, 999)]
    public int Quantity { get; set; }
}