using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("user_refresh_tokens");

        builder.HasKey(x => x.IdUserRefreshToken);

        builder.Property(x => x.IdUserRefreshToken)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.IdUser)
            .HasColumnName("id_user")
            .IsRequired();

        builder.Property(x => x.RefreshTokenHash)
            .HasColumnName("refresh_token_hash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .HasColumnName("expires_at_utc")
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.RevokedAtUtc)
            .HasColumnName("revoked_at_utc");

        builder.Property(x => x.IsRevoked)
            .HasColumnName("is_revoked")
            .IsRequired();

        builder.Property(x => x.CreatedByIp)
            .HasColumnName("created_by_ip");

        builder.Property(x => x.RevokedByIp)
            .HasColumnName("revoked_by_ip");

        builder.Property(x => x.DeviceName)
            .HasColumnName("device_name");

        builder.HasIndex(x => x.IdUser)
            .HasDatabaseName("IX_user_refresh_tokens_id_user");

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
    }
}