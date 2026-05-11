using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.Recipe;

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