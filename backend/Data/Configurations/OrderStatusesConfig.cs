using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class OrderStatusesConfig : IEntityTypeConfiguration<OrderStatuses>
{
    public void Configure(EntityTypeBuilder<OrderStatuses> builder)
    {
        builder.ToTable("order_statuses");
        builder.HasKey(c=>c.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(100);

        builder.HasIndex(i => i.Id);
        builder.HasIndex(i => i.Name).IsUnique();
    }
}