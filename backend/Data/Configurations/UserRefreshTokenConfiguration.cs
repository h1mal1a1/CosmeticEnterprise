using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("user_refresh_tokens");

        builder.HasKey(x => x.IdUserRefreshToken);

        builder.Property(x => x.RefreshTokenHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
    }
}