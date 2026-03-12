using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class LeftoversInWarehousesConfigure : IEntityTypeConfiguration<LeftoversInWarehouses>
{
    public void Configure(EntityTypeBuilder<LeftoversInWarehouses> builder)
    {
        builder.ToTable("leftovers_in_warehouses");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("Id");
        builder.HasOne(f => f.FinishedProducts).WithMany(f => f.LeftoversInWarehousesList);
        builder.HasOne(f => f.Warehouses).WithMany(f => f.LeftoversInWarehousesList);
    }
}