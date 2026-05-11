using CosmeticEnterpriseBack.Application.DTOs.Auth;

namespace CosmeticEnterpriseBack.Api.Services.Auth;

public interface IAuthCookieService
{
    void AppendAuthCookies(HttpResponse response, AuthResponse authResponse);
    void DeleteAuthCookies(HttpResponse respose);
}