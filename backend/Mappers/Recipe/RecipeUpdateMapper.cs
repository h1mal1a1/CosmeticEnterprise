using CosmeticEnterpriseBack.DTO.Recipe;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.Recipe;

public class RecipeUpdateMapper : IUpdateMapper<Recipes, UpdateRecipeRequest>
{
    public void Map(UpdateRecipeRequest source, Recipes entity)
    {
        entity.Name = source.Name;
    }
}