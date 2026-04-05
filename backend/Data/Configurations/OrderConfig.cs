using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderConfig : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.ToTable("orders");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.IdCustomer)
            .HasColumnName("id_customer");
        
        builder.Property(x => x.IdSalesChannel)
            .HasColumnName("id_sales_channel");
        
        builder.Property(x => x.IdOrderStatus)
            .HasColumnName("id_order_status");

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.OrdersList)
            .HasForeignKey(x => x.IdCustomer)
            .IsRequired();
        
        builder.HasOne(x => x.SalesChannel)
            .WithMany(x => x.OrdersList)
            .HasForeignKey(x => x.IdSalesChannel)
            .IsRequired();
        
        builder.HasOne(x => x.OrderStatus)
            .WithMany(x => x.OrdersList)
            .HasForeignKey(x => x.IdOrderStatus)
            .IsRequired();
    }
}