using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Application.DTOs.Recipe;
using CosmeticEnterpriseBack.Application.DTOs.UnitOfMeasurement;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Infrastructure.Mappers.FinishedProduct;
using CosmeticEnterpriseBack.Infrastructure.Mappers.ProductCategory;
using CosmeticEnterpriseBack.Infrastructure.Mappers.Recipe;
using CosmeticEnterpriseBack.Infrastructure.Mappers.UnitOfMeasurement;
using CosmeticEnterpriseBack.Mappers.UnitsOfMeasurement;
using CosmeticEnterpriseBack.Infrastructure.Readers;
using CosmeticEnterpriseBack.Infrastructure.Services;
using CosmeticEnterpriseBack.Application.Services.Order;
using CosmeticEnterpriseBack.Infrastructure.Services.UserAddresses;
using CosmeticEnterpriseBack.Infrastructure.Services.Order;
using CosmeticEnterpriseBack.Application.Mappers;

namespace CosmeticEnterpriseBack.Infrastructure.Extensions;

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
        services.AddScoped<IOrderMapper, OrderMapper>();
        services.AddScoped<IOrderDictionaryService, OrderDictionaryService>();
        services.AddScoped<IOrderQueryBuilder, OrderQueryBuilder>();
        services.AddScoped<IOrderStockService, OrderStockService>();
        services.AddScoped<IOrderReadService, OrderReadService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserAddressService, UserAddressService>();

        AddFP(services);
        AddPC(services);
        AddRecipes(services);
        AddUnitsOfMeasurement(services);

        services.AddScoped<ICrudServiceFactory, CrudServiceFactory>();

        return services;
    }
}