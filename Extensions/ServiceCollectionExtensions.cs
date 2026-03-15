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

        services.AddScoped<
            ICrudService<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long>,
            CrudService<FinishedProducts, long, CreateFinishedProductRequest, UpdateFinishedProductRequest,
                FinishedProductResponse>>();

        return services;
    }
}