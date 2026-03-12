using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class MaterialsConfigure : IEntityTypeConfiguration<Materials>
{
    public void Configure(EntityTypeBuilder<Materials> builder)
    {
        builder.ToTable("materials");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.HasOne(f=>f.UnitsOfMeasurement).WithMany(f=>f.MaterialsList);
    }
}