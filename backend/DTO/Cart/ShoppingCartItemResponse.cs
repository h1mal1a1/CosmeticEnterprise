namespace CosmeticEnterpriseBack.DTO.Cart;

public class ShoppingCartItemResponse
{
    public long Id { get; set; }
    public long IdFinishedProduct { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public bool HasEnoughStock { get; set; }
    public decimal LineTotal { get; set; }
}