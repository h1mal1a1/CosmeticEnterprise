using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("user_addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdUser)
            .HasColumnName("id_user")
            .IsRequired();

        builder.Property(x => x.RecipientName)
            .HasColumnName("recipient_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.City)
            .HasColumnName("city")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Street)
            .HasColumnName("street")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.House)
            .HasColumnName("house")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Apartment)
            .HasColumnName("apartment")
            .HasMaxLength(50);

        builder.Property(x => x.PostalCode)
            .HasColumnName("postal_code")
            .HasMaxLength(20);

        builder.Property(x => x.Comment)
            .HasColumnName("comment")
            .HasMaxLength(1000);

        builder.Property(x => x.IsDefault)
            .HasColumnName("is_default")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.HasIndex(x => x.IdUser)
            .HasDatabaseName("IX_user_addresses_id_user");

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
    }
}