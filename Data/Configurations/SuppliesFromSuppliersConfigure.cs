using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SuppliesFromSuppliersConfigure : IEntityTypeConfiguration<SuppliesFromSuppliers>
{
    public void Configure(EntityTypeBuilder<SuppliesFromSuppliers> builder)
    {
        builder.ToTable("supplies_from_suppliers");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.HasOne(f => f.Supplier).WithMany(f => f.SuppliesFromSuppliersList);
    }
}