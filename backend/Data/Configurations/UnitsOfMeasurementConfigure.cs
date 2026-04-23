using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class UnitsOfMeasurementConfigure : IEntityTypeConfiguration<UnitsOfMeasurement>
{
    public void Configure(EntityTypeBuilder<UnitsOfMeasurement> builder)
    {
        builder.ToTable("units_of_measurement");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_units_of_measurement_name")
            .IsUnique();
    }
}