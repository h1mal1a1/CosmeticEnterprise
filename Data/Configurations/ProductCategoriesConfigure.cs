using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class ProductCategoriesConfigure : IEntityTypeConfiguration<ProductCategories>
{
    public void Configure(EntityTypeBuilder<ProductCategories> builder)
    {
        builder.ToTable("product_categories");
        
        builder.HasKey(f => f.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}