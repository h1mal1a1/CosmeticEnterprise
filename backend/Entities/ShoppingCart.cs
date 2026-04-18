namespace CosmeticEnterpriseBack.Entities;

public sealed class ShoppingCart
{
    public long Id { get; set; }
    public long IdUser { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public User User { get; set; } = null!;
    public ICollection<ShoppingCartItem> Items { get; set; } = [];
}