using CosmeticEnterpriseBack.Api.Services.Auth;
using CosmeticEnterpriseBack.Api.Services.CurrentUser;
using CosmeticEnterpriseBack.Application.Interfaces;

namespace CosmeticEnterpriseBack.Api.Extensions;

public static class ApiServiceExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {        
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}