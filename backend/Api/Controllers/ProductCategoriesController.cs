using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Route("api/product-categories")]
public class ProductCategoriesController(ICrudServiceFactory crudFactory)
    : CrudController<ProductCategoryResponse, CreateProductCategoryRequest, UpdateProductCategoryRequest, long>
    (crudFactory.CreateProductCategoriesService())
{
}