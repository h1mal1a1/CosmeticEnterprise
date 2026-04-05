using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class WarehousesConfigure : IEntityTypeConfiguration<Warehouses>
{
    public void Configure(EntityTypeBuilder<Warehouses> builder)
    {
        builder.ToTable("warehouses");
        builder.HasKey(f => f.Id);
    }
}