using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public sealed class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.ToTable("shopping_cart_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdShoppingCart)
            .HasColumnName("id_shopping_cart")
            .IsRequired();

        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.HasIndex(x => new { x.IdShoppingCart, x.IdFinishedProduct })
            .IsUnique();

        builder.HasOne(x => x.ShoppingCart)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.IdShoppingCart)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FinishedProduct)
            .WithMany()
            .HasForeignKey(x => x.IdFinishedProduct)
            .OnDelete(DeleteBehavior.Restrict);
    }
}