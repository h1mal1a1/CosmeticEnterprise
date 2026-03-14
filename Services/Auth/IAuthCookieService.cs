using CosmeticEnterpriseBack.DTO.Auth;

namespace CosmeticEnterpriseBack.Services.Auth;

public interface IAuthCookieService
{
    void AppendAuthCookies(HttpResponse response, AuthResponse authResponse);
    void DeleteAuthCookies(HttpResponse respose);
}