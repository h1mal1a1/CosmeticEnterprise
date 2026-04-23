using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class MaterialsConfigure : IEntityTypeConfiguration<Materials>
{
    public void Configure(EntityTypeBuilder<Materials> builder)
    {
        builder.ToTable("materials");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdUnitsOfMeasurement)
            .HasColumnName("id_units_of_measurement")
            .IsRequired();

        builder.HasIndex(x => x.IdUnitsOfMeasurement)
            .HasDatabaseName("IX_materials_id_units_of_measurement");

        builder.HasOne(x => x.UnitsOfMeasurement)
            .WithMany(x => x.MaterialsList)
            .HasForeignKey(x => x.IdUnitsOfMeasurement)
            .OnDelete(DeleteBehavior.Cascade);
    }
}