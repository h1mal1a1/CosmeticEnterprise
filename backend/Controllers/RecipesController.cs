using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.Controllers.Base;
using CosmeticEnterpriseBack.DTO.Recipe;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Authorize]
[Route("api/recipes")]
public class RecipesController
    : CrudController<RecipeResponse, CreateRecipeRequest, UpdateRecipeRequest, long>
{
    public RecipesController(ICrudServiceFactory crudServiceFactory)
        : base(
            crudServiceFactory.Create<
                Recipes,
                long,
                CreateRecipeRequest,
                UpdateRecipeRequest,
                RecipeResponse>(ResourceType.Recipe))
    {
    }
}