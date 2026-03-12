using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SupplyPositionsConfigure : IEntityTypeConfiguration<SupplyPositions>
{
    public void Configure(EntityTypeBuilder<SupplyPositions> builder)
    {
        builder.ToTable("supply_positions");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.HasOne(f=>f.SuppliesFromSuppliers).WithMany(f=>f.SupplyPositionsList);
        builder.HasOne(f=>f.Material).WithMany(f=>f.SupplyPositionsList);
    }
}