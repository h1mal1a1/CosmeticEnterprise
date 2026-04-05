using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class CustomersConfig : IEntityTypeConfiguration<Customers>
{
    public void Configure(EntityTypeBuilder<Customers> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c=>c.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(100);

        builder.HasIndex(i => i.Id);
        builder.HasIndex(i => i.Name).IsUnique();
        builder.HasMany(c => c.OrdersList).WithOne(o => o.Customer);
    }
}