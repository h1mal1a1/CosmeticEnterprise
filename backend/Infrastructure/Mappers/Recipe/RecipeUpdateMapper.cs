using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.Recipe;

public class RecipeUpdateMapper : IUpdateMapper<Recipes, UpdateRecipeRequest>
{
    public void Map(UpdateRecipeRequest source, Recipes entity)
    {
        entity.Name = source.Name;
    }
}