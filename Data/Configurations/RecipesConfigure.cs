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
    }
}