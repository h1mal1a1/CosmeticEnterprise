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
    }
}