using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Authorization;

namespace CosmeticEnterpriseBack.Api.Controllers;

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