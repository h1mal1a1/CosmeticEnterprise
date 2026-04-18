namespace CosmeticEnterpriseBack.DTO.Cart;

public class ShoppingCartResponse
{
    public long Id { get; set; }
    public long IdUser { get; set; }
    public List<ShoppingCartItemResponse> Items { get; set; } = [];
    public int TotalItemsQuantity { get; set; }
    public decimal TotalAmount { get; set; }
}