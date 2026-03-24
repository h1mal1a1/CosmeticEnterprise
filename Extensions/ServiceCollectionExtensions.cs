using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Mappers.FinishedProduct;
using CosmeticEnterpriseBack.Mappers.ProductCategory;
using CosmeticEnterpriseBack.Readers;
using CosmeticEnterpriseBack.Services.Crud;

namespace CosmeticEnterpriseBack.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddFP(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateMapper<FinishedProducts, CreateFinishedProductRequest>, FinishedProductCreateMapper>();
        services
            .AddScoped<IUpdateMapper<FinishedProducts, UpdateFinishedProductRequest>, FinishedProductUpdateMapper>();
        services
            .AddScoped<IResponseMapper<FinishedProducts, FinishedProductResponse>, FinishedProductResponseMapper>();
    }

    private static void AddPC(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateMapper<ProductCategories, CreateProductCategoryRequest>, ProductCategoryCreateMapper>();
        services
            .AddScoped<IUpdateMapper<ProductCategories, UpdateProductCategoryRequest>, ProductCategoryUpdateMapper>();
        services
            .AddScoped<IResponseMapper<ProductCategories, ProductCategoryResponse>, ProductCategoryResponseMapper>();
    }
    public static IServiceCollection AddCrudServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityReader<,>), typeof(EntityReader<,>));
        
        AddFP(services);
        AddPC(services);
        
        services.AddScoped<ICrudServiceFactory, CrudServiceFactory>();

        return services;
    }
}