using CosmeticEnterpriseBack.Base;
using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Mappers.FinishedProduct;
using CosmeticEnterpriseBack.Readers;

namespace CosmeticEnterpriseBack.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrudServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityReader<,>), typeof(EntityReader<,>));
        services
            .AddScoped<ICreateMapper<FinishedProducts, CreateFinishedProductRequest>, FinishedProductCreateMapper>();
        services
            .AddScoped<IUpdateMapper<FinishedProducts, UpdateFinishedProductRequest>, FinishedProductUpdateMapper>();
        services
            .AddScoped<IResponseMapper<FinishedProducts, FinishedProductResponse>, FinishedProductResponseMapper>();

        services
            .AddCrud<FinishedProducts, FinishedProductResponse, CreateFinishedProductRequest,
                UpdateFinishedProductRequest, long>();

        return services;
    }

    public static IServiceCollection AddCrud<TEntity, TResponse, TCreateRequest, TUpdateRequest, TKey>(
        this IServiceCollection services) where TEntity : class
    {
        services.AddScoped<ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey>,
            CrudService<TEntity, TKey, TCreateRequest, TUpdateRequest, TResponse>>();
        return services;
    }
}