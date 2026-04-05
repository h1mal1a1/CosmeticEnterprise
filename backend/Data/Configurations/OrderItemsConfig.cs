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

        builder.Property(x => x.IdOrder)
            .HasColumnName("id_order");
        
        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product");
        
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItemsList)
            .HasForeignKey(x=>x.IdOrder);
        
        builder.HasOne(x => x.FinishedProducts)
            .WithMany(x => x.OrderItemsList)
            .HasForeignKey(x=>x.IdFinishedProduct);
    }
}