using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.Recipe;

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