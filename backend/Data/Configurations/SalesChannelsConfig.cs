using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class SalesChannelsConfig : IEntityTypeConfiguration<SalesChannels>
{
    public void Configure(EntityTypeBuilder<SalesChannels> builder)
    {
        builder.ToTable("sales_channels");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_sales_channels_name")
            .IsUnique();
    }
}