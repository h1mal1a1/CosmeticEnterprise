using CosmeticEnterpriseBack.DTO.Recipe;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.Recipe;

public class RecipeResponseMapper : IResponseMapper<Recipes, RecipeResponse>
{
    public RecipeResponse Map(Recipes source)
    {
        return new RecipeResponse
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}