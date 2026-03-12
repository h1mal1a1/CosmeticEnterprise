using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderItemsConfig : IEntityTypeConfiguration<OrderItems>
{
    public void Configure(EntityTypeBuilder<OrderItems> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(oi => oi.Id);
        builder.HasOne(oi => oi.Order).WithMany(o => o.LstOrderItems);
        builder.HasOne(oi => oi.FinishedProducts).WithMany(o => o.OrderItemsList);
    }
}