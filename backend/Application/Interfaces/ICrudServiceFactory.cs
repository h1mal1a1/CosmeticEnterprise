using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Application.DTOs.UnitOfMeasurement;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface ICrudServiceFactory
{
    ICrudService<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long>
        CreateFinishedProductsService();

    ICrudService<ProductCategoryResponse, CreateProductCategoryRequest, UpdateProductCategoryRequest, long>
        CreateProductCategoriesService();

    ICrudService<RecipeResponse, CreateRecipeRequest, UpdateRecipeRequest, long>
        CreateRecipesService();

    ICrudService<UnitOfMeasurementResponse, CreateUnitOfMeasurementRequest, UpdateUnitOfMeasurementRequest, long>
        CreateUnitsOfMeasurementService();
}