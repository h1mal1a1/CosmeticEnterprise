using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class ProductCategoriesConfigure : IEntityTypeConfiguration<ProductCategories>
{
    public void Configure(EntityTypeBuilder<ProductCategories> builder)
    {
        builder.ToTable("product_categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_product_categories_name")
            .IsUnique();
    }
}