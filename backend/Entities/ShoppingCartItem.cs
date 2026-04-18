namespace CosmeticEnterpriseBack.Entities;

public sealed class ShoppingCartItem
{
    public long Id { get; set; }
    public long IdShoppingCart { get; set; }
    public long IdFinishedProduct { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
    public FinishedProducts FinishedProduct { get; set; } = null!;
}