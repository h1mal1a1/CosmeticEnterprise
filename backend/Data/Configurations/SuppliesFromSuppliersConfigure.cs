using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SuppliesFromSuppliersConfigure : IEntityTypeConfiguration<SuppliesFromSuppliers>
{
    public void Configure(EntityTypeBuilder<SuppliesFromSuppliers> builder)
    {
        builder.ToTable("supplies_from_suppliers");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdSupplier)
            .HasColumnName("id_supplier");

        builder.HasOne(x => x.Supplier)
            .WithMany(x => x.SuppliesFromSuppliersList)
            .HasForeignKey(x => x.IdSupplier);
    }
}