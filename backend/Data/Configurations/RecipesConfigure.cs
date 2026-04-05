using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticEnterpriseBack.Data.Configurations;

public class RecipesConfigure : IEntityTypeConfiguration<Recipes>
{
    public void Configure(EntityTypeBuilder<Recipes> builder)
    {
        builder.ToTable("recipes");
        
        builder.HasKey(f => f.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}