using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Interfaces.Persistence;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Application.Services;
using CosmeticEnterpriseBack.Application.UseCases.FinishedProducts.UpdateStock;
using CosmeticEnterpriseBack.Application.Validators;
using CosmeticEnterpriseBack.Domain.Services;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;
using CosmeticEnterpriseBack.Infrastructure.Persistence;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Repositories;
using CosmeticEnterpriseBack.Infrastructure.Services.Auth;
using CosmeticEnterpriseBack.Infrastructure.Services.Cart;
using CosmeticEnterpriseBack.Infrastructure.Services.CurrentUser;
using CosmeticEnterpriseBack.Infrastructure.Services.FinishedProductImages;

namespace CosmeticEnterpriseBack.Infrastructure.Extensions;

public static class ApplicationServiceExtensions 
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddScoped<IFinishedProductImageService, FinishedProductImageService>();
        services.AddScoped<ICartService, CartService>();

        services.AddScoped<IUserAddressValidator, UserAddressValidator>();
        services.AddScoped<IOrderStatusTransitionValidator, OrderStatusTransitionValidator>();
        services.AddScoped<IOrderReturnUrlValidator, OrderReturnUrlValidator>();

        services.AddScoped<UserAddressDomainService>();
        services.AddScoped<IUserAddressMapper, UserAddressMapper>();
        services.AddScoped<IUserAddressService, UserAddressAppService>();
        services.AddScoped<IUserAddressRepository, UserAddressRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUpdateFinishedProductStockUseCase, UpdateFinishedProductStockUseCase>();
        services.AddScoped<IFinishedProductStockRepository, FinishedProductStockRepository>();

        services.AddCrudServices();
    }
}