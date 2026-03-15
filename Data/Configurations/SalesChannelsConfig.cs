using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SalesChannelsConfig : IEntityTypeConfiguration<SalesChannels>
{
    public void Configure(EntityTypeBuilder<SalesChannels> builder)
    {
        builder.ToTable("sales_channels");
        builder.HasKey(c=>c.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(100);

        builder.HasIndex(i => i.Id);
        builder.HasIndex(i => i.Name).IsUnique();
    }
}