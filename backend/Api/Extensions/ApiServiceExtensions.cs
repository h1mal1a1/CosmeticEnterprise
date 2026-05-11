using CosmeticEnterpriseBack.Api.Services.Auth;

namespace CosmeticEnterpriseBack.Api.Extensions;

public static class ApiServiceExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthCookieService, AuthCookieService>();
    }
}