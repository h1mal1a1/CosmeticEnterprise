using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class UnitsOfMeasurementConfigure : IEntityTypeConfiguration<UnitsOfMeasurement>
{
    public void Configure(EntityTypeBuilder<UnitsOfMeasurement> builder)
    {
        builder.ToTable("units_of_measurement");
        builder.HasKey(f => f.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}