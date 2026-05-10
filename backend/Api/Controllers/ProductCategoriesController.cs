using CosmeticEnterpriseBack.Infrastructure.Authorization;
using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Api.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Route("api/product-categories")]
public class ProductCategoriesController :
    CrudController<
        ProductCategoryResponse, 
        CreateProductCategoryRequest, 
        UpdateProductCategoryRequest, 
        long>
{
    public ProductCategoriesController(ICrudServiceFactory crudFactory)
        : base(
            crudFactory.Create<
                ProductCategories, 
                long, 
                CreateProductCategoryRequest, 
                UpdateProductCategoryRequest, 
                ProductCategoryResponse>(ResourceType.ProductCategory))
    {
        
    }
    
}