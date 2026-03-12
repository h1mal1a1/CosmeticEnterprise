using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class ProductPartiesConfigure : IEntityTypeConfiguration<ProductParties>
{
    public void Configure(EntityTypeBuilder<ProductParties> builder)
    {
        builder.ToTable("product_parties");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.HasOne(f => f.FinishedProducts).WithMany(f => f.ProductPartiesList);
    }
}