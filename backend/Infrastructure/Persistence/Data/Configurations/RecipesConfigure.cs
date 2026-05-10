using CosmeticEnterpriseBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Infrastructure.Persistence.Data.Configurations;

public class RecipesConfigure : IEntityTypeConfiguration<Recipes>
{
    public void Configure(EntityTypeBuilder<Recipes> builder)
    {
        builder.ToTable("recipes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_recipes_name")
            .IsUnique();
    }
}