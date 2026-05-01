using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class FinishedProductsConfigure : IEntityTypeConfiguration<FinishedProducts>
{
    public void Configure(EntityTypeBuilder<FinishedProducts> builder)
    {
        builder.ToTable("finished_products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.WbUrl)
            .HasColumnName("wb_url")
            .HasMaxLength(1000);

        builder.Property(x => x.IdRecipe)
            .HasColumnName("id_recipe")
            .IsRequired();

        builder.Property(x => x.IdProductCategory)
            .HasColumnName("id_product_category")
            .IsRequired();

        builder.Property(x => x.IdUnitsOfMeasurement)
            .HasColumnName("id_units_of_measurement")
            .IsRequired();

        builder.HasIndex(x => x.IdRecipe)
            .HasDatabaseName("IX_finished_products_id_recipe");

        builder.HasIndex(x => x.IdProductCategory)
            .HasDatabaseName("IX_finished_products_id_product_category");

        builder.HasIndex(x => x.IdUnitsOfMeasurement)
            .HasDatabaseName("IX_finished_products_id_units_of_measurement");

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.FinishedProductsList)
            .HasForeignKey(x => x.IdRecipe)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ProductCategories)
            .WithMany(x => x.FinishedProductsList)
            .HasForeignKey(x => x.IdProductCategory)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UnitsOfMeasurement)
            .WithMany(x => x.FinishedProductsList)
            .HasForeignKey(x => x.IdUnitsOfMeasurement)
            .OnDelete(DeleteBehavior.Cascade);
    }
}