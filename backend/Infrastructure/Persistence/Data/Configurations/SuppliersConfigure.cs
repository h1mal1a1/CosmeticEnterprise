using CosmeticEnterpriseBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Infrastructure.Persistence.Data.Configurations;

public class SuppliersConfigure : IEntityTypeConfiguration<Suppliers>
{
    public void Configure(EntityTypeBuilder<Suppliers> builder)
    {
        builder.ToTable("suppliers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();
    }
}