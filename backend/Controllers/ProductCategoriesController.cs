using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Controllers.Base;
using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

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