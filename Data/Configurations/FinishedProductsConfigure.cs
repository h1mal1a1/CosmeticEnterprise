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
        builder.HasOne(f => f.Recipe).WithMany(f => f.FinishedProductsList);
        builder.HasOne(f => f.ProductCategories).WithMany(f => f.FinishedProductsList);
        builder.HasOne(f => f.UnitsOfMeasurement).WithMany(f => f.FinishedProductsList);
    }
}