using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class FinishedProductImagesConfig : IEntityTypeConfiguration<FinishedProductImages>
{
    public void Configure(EntityTypeBuilder<FinishedProductImages> builder)
    {
        builder.ToTable("product_images");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product")
            .IsRequired();

        builder.Property(x => x.ObjectKey)
            .HasColumnName("object_key")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.SortOrder)
            .HasColumnName("sort_order")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.IsMain)
            .HasColumnName("is_main")
            .IsRequired();

        builder.HasOne(x => x.FinishedProduct)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.IdFinishedProduct)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.IdFinishedProduct)
            .HasDatabaseName("IX_product_images_id_finished_product");

        builder.HasIndex(x => new { x.IdFinishedProduct, x.IsMain })
            .HasDatabaseName("IX_product_images_id_finished_product_is_main")
            .HasFilter("\"is_main\" = true")
            .IsUnique();
    }
}