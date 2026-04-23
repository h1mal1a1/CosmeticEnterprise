using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.DTO.Recipe;
using CosmeticEnterpriseBack.DTO.UnitOfMeasurement;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Mappers.FinishedProduct;
using CosmeticEnterpriseBack.Mappers.ProductCategory;
using CosmeticEnterpriseBack.Mappers.Recipe;
using CosmeticEnterpriseBack.Mappers.UnitOfMeasurement;
using CosmeticEnterpriseBack.Mappers.UnitsOfMeasurement;
using CosmeticEnterpriseBack.Readers;
using CosmeticEnterpriseBack.Services.Crud;
using CosmeticEnterpriseBack.Services.Order;
using CosmeticEnterpriseBack.Services.UserAddresses;

namespace CosmeticEnterpriseBack.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddFP(this IServiceCollection services)
    {
        services.AddScoped<IEntityReader<FinishedProducts, long>, FinishedProductsReader>();
        services.AddScoped<ICreateMapper<FinishedProducts, CreateFinishedProductRequest>, FinishedProductCreateMapper>();
        services.AddScoped<IUpdateMapper<FinishedProducts, UpdateFinishedProductRequest>, FinishedProductUpdateMapper>();
        services.AddScoped<IResponseMapper<FinishedProducts, FinishedProductResponse>, FinishedProductResponseMapper>();
    }

    private static void AddPC(this IServiceCollection services)
    {
        services.AddScoped<ICreateMapper<ProductCategories, CreateProductCategoryRequest>, ProductCategoryCreateMapper>();
        services.AddScoped<IUpdateMapper<ProductCategories, UpdateProductCategoryRequest>, ProductCategoryUpdateMapper>();
        services.AddScoped<IResponseMapper<ProductCategories, ProductCategoryResponse>, ProductCategoryResponseMapper>();
    }

    private static void AddRecipes(this IServiceCollection services)
    {
        services.AddScoped<ICreateMapper<Recipes, CreateRecipeRequest>, RecipeCreateMapper>();
        services.AddScoped<IUpdateMapper<Recipes, UpdateRecipeRequest>, RecipeUpdateMapper>();
        services.AddScoped<IResponseMapper<Recipes, RecipeResponse>, RecipeResponseMapper>();
    }

    private static void AddUnitsOfMeasurement(this IServiceCollection services)
    {
        services.AddScoped<ICreateMapper<UnitsOfMeasurement, CreateUnitOfMeasurementRequest>, UnitOfMeasurementCreateMapper>();
        services.AddScoped<IUpdateMapper<UnitsOfMeasurement, UpdateUnitOfMeasurementRequest>, UnitOfMeasurementUpdateMapper>();
        services.AddScoped<IResponseMapper<UnitsOfMeasurement, UnitOfMeasurementResponse>, UnitOfMeasurementResponseMapper>();
    }

    public static IServiceCollection AddCrudServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityReader<,>), typeof(EntityReader<,>));
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderDictionaryService, OrderDictionaryService>();
        services.AddScoped<IUserAddressService, UserAddressService>();

        AddFP(services);
        AddPC(services);
        AddRecipes(services);
        AddUnitsOfMeasurement(services);

        services.AddScoped<ICrudServiceFactory, CrudServiceFactory>();

        return services;
    }
}