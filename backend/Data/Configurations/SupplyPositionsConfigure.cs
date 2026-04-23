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

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdSuppliesFromSupplier)
            .HasColumnName("id_supplies_from_supplier")
            .IsRequired();

        builder.Property(x => x.IdMaterial)
            .HasColumnName("id_material")
            .IsRequired();

        builder.HasIndex(x => x.IdSuppliesFromSupplier)
            .HasDatabaseName("IX_supply_positions_id_supplies_from_supplier");

        builder.HasIndex(x => x.IdMaterial)
            .HasDatabaseName("IX_supply_positions_id_material");

        builder.HasOne(x => x.SuppliesFromSuppliers)
            .WithMany(x => x.SupplyPositionsList)
            .HasForeignKey(x => x.IdSuppliesFromSupplier)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Material)
            .WithMany(x => x.SupplyPositionsList)
            .HasForeignKey(x => x.IdMaterial)
            .OnDelete(DeleteBehavior.Cascade);
    }
}