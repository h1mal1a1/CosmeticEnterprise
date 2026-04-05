using CosmeticEnterpriseBack.DTO.Recipe;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.Recipe;

public class RecipeCreateMapper : ICreateMapper<Recipes, CreateRecipeRequest>
{
    public Recipes Map(CreateRecipeRequest source)
    {
        return new Recipes
        {
            Name = source.Name
        };
    }
}