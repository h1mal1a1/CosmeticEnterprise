using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class LeftoversInWarehousesConfigure : IEntityTypeConfiguration<LeftoversInWarehouses>
{
    public void Configure(EntityTypeBuilder<LeftoversInWarehouses> builder)
    {
        builder.ToTable("leftovers_in_warehouses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdFinishedProduct)
            .HasColumnName("id_finished_product");

        builder.Property(x => x.IdWarehouse)
            .HasColumnName("id_warehouse");

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .HasColumnName("reserved_quantity")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne(x => x.FinishedProducts)
            .WithMany(x => x.LeftoversInWarehousesList)
            .HasForeignKey(x => x.IdFinishedProduct);

        builder.HasOne(x => x.Warehouses)
            .WithMany(x => x.LeftoversInWarehousesList)
            .HasForeignKey(x => x.IdWarehouse);
    }
}