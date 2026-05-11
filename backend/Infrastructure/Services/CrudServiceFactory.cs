using CosmeticEnterpriseBack.Application.Authorization;
using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Application.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;

namespace CosmeticEnterpriseBack.Infrastructure.Services;

public class CrudServiceFactory(
    AppDbContext dbContext,
    IServiceProvider serviceProvider,
    IAuthorizationService authorizationService) : ICrudServiceFactory
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    public ICrudService<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long>
        CreateFinishedProductsService()
    {
        return Create<FinishedProducts, long, CreateFinishedProductRequest, UpdateFinishedProductRequest,
            FinishedProductResponse>(ResourceType.FinishedProduct);
    }

    public ICrudService<ProductCategoryResponse, CreateProductCategoryRequest, UpdateProductCategoryRequest, long>
        CreateProductCategoriesService()
    {
        return Create<ProductCategories, long, CreateProductCategoryRequest, UpdateProductCategoryRequest,
            ProductCategoryResponse>(ResourceType.ProductCategory);
    }

    public ICrudService<RecipeResponse, CreateRecipeRequest, UpdateRecipeRequest, long>
        CreateRecipesService()
    {
        return Create<Recipes, long, CreateRecipeRequest, UpdateRecipeRequest, RecipeResponse>(ResourceType.Recipe);
    }

    public ICrudService<UnitOfMeasurementResponse, CreateUnitOfMeasurementRequest, UpdateUnitOfMeasurementRequest, long>
        CreateUnitsOfMeasurementService()
    {
        return Create<UnitsOfMeasurement, long, CreateUnitOfMeasurementRequest, UpdateUnitOfMeasurementRequest,
            UnitOfMeasurementResponse>(ResourceType.UnitOfMeasurement);
    }

    private ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>
        Create<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(ResourceType resourceType)
        where TEntity : class
    {
        var reader = _serviceProvider.GetRequiredService<IEntityReader<TEntity, TKey>>();
        var createMapper = _serviceProvider.GetRequiredService<ICreateMapper<TEntity, TCreateRequest>>();
        var updateMapper = _serviceProvider.GetRequiredService<IUpdateMapper<TEntity, TUpdateRequest>>();
        var responseMapper = _serviceProvider.GetRequiredService<IResponseMapper<TEntity, TResponse>>();

        return new CrudService<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>(
            _dbContext,
            reader,
            createMapper,
            updateMapper,
            responseMapper,
            _authorizationService,
            resourceType);
    }
}