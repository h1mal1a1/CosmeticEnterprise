using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderItemsConfig : IEntityTypeConfiguration<OrderItems>
{
    public void Configure(EntityTypeBuilder<OrderItems> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdOrder)
            .HasColumnName("id_order")
            .IsRequired();

        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.LineTotal)
            .HasColumnName("line_total")
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.HasIndex(x => x.IdOrder)
            .HasDatabaseName("IX_order_items_id_order");

        builder.HasIndex(x => x.IdFinishedProduct)
            .HasDatabaseName("IX_order_items_id_finished_product");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItemsList)
            .HasForeignKey(x => x.IdOrder)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FinishedProducts)
            .WithMany(x => x.OrderItemsList)
            .HasForeignKey(x => x.IdFinishedProduct)
            .OnDelete(DeleteBehavior.Restrict);
    }
}