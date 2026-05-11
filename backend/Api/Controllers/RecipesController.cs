using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/recipes")]
public class RecipesController(ICrudServiceFactory crudFactory)
    : CrudController<RecipeResponse, CreateRecipeRequest, UpdateRecipeRequest, long>(crudFactory.CreateRecipesService())
{
}