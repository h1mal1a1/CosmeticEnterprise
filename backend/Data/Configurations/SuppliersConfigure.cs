using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SuppliersConfigure : IEntityTypeConfiguration<Suppliers>
{
    public void Configure(EntityTypeBuilder<Suppliers> builder)
    {
        builder.ToTable("suppliers");
        builder.HasKey(f => f.Id);
    }
}