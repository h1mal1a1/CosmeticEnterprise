using CosmeticEnterpriseBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Infrastructure.Persistence.Data.Configurations;

public class WarehousesConfigure : IEntityTypeConfiguration<Warehouses>
{
    public void Configure(EntityTypeBuilder<Warehouses> builder)
    {
        builder.ToTable("warehouses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();
    }
}