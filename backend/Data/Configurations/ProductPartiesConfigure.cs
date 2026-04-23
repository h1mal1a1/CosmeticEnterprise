using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class ProductPartiesConfigure : IEntityTypeConfiguration<ProductParties>
{
    public void Configure(EntityTypeBuilder<ProductParties> builder)
    {
        builder.ToTable("product_parties");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product")
            .IsRequired();

        builder.HasIndex(x => x.IdFinishedProduct)
            .HasDatabaseName("IX_product_parties_id_finished_product");

        builder.HasOne(x => x.FinishedProducts)
            .WithMany(x => x.ProductPartiesList)
            .HasForeignKey(x => x.IdFinishedProduct)
            .OnDelete(DeleteBehavior.Cascade);
    }
}