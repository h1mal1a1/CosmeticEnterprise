using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Services.Auth;
using CosmeticEnterpriseBack.Services.Cart;
using CosmeticEnterpriseBack.Services.CurrentUser;
using CosmeticEnterpriseBack.Services.FinishedProductImages;

namespace CosmeticEnterpriseBack.Extensions;

public static class ApplicationServiceExtensions 
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthCookieService, AuthCookieService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

        builder.Services.AddScoped<IFinishedProductImageService, FinishedProductImageService>();
        builder.Services.AddScoped<ICartService, CartService>();

        builder.Services.AddCrudServices();
    }
}