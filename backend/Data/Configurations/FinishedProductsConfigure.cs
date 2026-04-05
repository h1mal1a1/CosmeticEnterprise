using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class FinishedProductsConfigure : IEntityTypeConfiguration<FinishedProducts>
{
    public void Configure(EntityTypeBuilder<FinishedProducts> builder)
    {
        builder.ToTable("finished_products");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.IdRecipe).HasColumnName("id_recipe");
        builder.Property(f => f.IdProductCategory).HasColumnName("id_product_category");
        builder.Property(f => f.IdUnitsOfMeasurement).HasColumnName("id_units_of_measurement");

        builder.HasOne(f => f.Recipe)
            .WithMany(f => f.FinishedProductsList)
            .HasForeignKey(f => f.IdRecipe);

        builder.HasOne(f => f.ProductCategories)
            .WithMany(f => f.FinishedProductsList)
            .HasForeignKey(f => f.IdProductCategory);

        builder.HasOne(f => f.UnitsOfMeasurement)
            .WithMany(f => f.FinishedProductsList)
            .HasForeignKey(f => f.IdUnitsOfMeasurement);
    }
}