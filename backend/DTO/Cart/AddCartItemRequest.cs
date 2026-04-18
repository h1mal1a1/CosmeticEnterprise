using System.ComponentModel.DataAnnotations;

namespace CosmeticEnterpriseBack.DTO.Cart;

public class AddCartItemRequest
{
    [Range(1, long.MaxValue)]
    public long IdFinishedProduct { get; set; }
    [Range(1, 999)]
    public int Quantity { get; set; }
}