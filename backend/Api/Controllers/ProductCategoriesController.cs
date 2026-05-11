using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CosmeticEnterpriseBack.Application.Authorization;
using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;

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