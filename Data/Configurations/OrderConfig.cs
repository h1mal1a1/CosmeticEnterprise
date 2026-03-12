using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderConfig : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(c=>c.Id);
        builder.Property(p => p.IdCustomers).HasColumnName("IdCustomers");
        builder.Property(p => p.IdSalesChannels).HasColumnName("IdSalesChannels");
        builder.Property(p => p.IdOrderStatuses).HasColumnName("IdOrderStatuses");
            
        builder.HasOne(o=>o.Customer)
            .WithMany(c => c.LstOrders)
            .HasForeignKey(o => o.IdCustomers)
            .HasPrincipalKey(c=>c.Id)
            .IsRequired();
        builder.HasOne(o=>o.SalesChannel)
            .WithMany(c => c.LstOrders)
            .HasForeignKey(o => o.IdSalesChannels)
            .HasPrincipalKey(c=>c.Id)
            .IsRequired();
        builder.HasOne(o=>o.OrderStatus)
            .WithMany(c => c.LstOrders)
            .HasForeignKey(o => o.IdOrderStatuses)
            .HasPrincipalKey(c=>c.Id)
            .IsRequired();
        builder.HasMany(o  => o.LstOrderItems).WithOne(oi => oi.Order);
    }
}