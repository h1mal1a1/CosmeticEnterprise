using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SupplyPositionsConfigure : IEntityTypeConfiguration<SupplyPositions>
{
    public void Configure(EntityTypeBuilder<SupplyPositions> builder)
    {
        builder.ToTable("supply_positions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdSuppliesFromSupplier)
            .HasColumnName("id_supplies_from_supplier");

        builder.Property(x => x.IdMaterial)
            .HasColumnName("id_material");

        builder.HasOne(x => x.SuppliesFromSuppliers)
            .WithMany(x => x.SupplyPositionsList)
            .HasForeignKey(x => x.IdSuppliesFromSupplier);

        builder.HasOne(x => x.Material)
            .WithMany(x => x.SupplyPositionsList)
            .HasForeignKey(x => x.IdMaterial);
    }
}