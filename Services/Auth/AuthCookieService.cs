using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.DTO.Auth;
using Microsoft.Extensions.Options;

namespace CosmeticEnterpriseBack.Services.Auth;

public class AuthCookieService : IAuthCookieService
{
    private readonly JwtSettings _jwtSettings;
    public AuthCookieService(IOptions<JwtSettings> jwtOptions) => _jwtSettings = jwtOptions.Value;

    public void AppendAuthCookies(HttpResponse response, AuthResponse authResponse)
    {
        var accessCookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeMinutes)
        };
        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays)
        };
        
        response.Cookies.Append(AuthCookieNames.AccessToken, authResponse.AccessToken,accessCookieOptions);
        response.Cookies.Append(AuthCookieNames.RefreshToken, authResponse.RefreshToken,refreshCookieOptions);
    }

    public void DeleteAuthCookies(HttpResponse response)
    {
        var deleteCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        };
        response.Cookies.Delete(AuthCookieNames.AccessToken, deleteCookieOptions);
        response.Cookies.Delete(AuthCookieNames.RefreshToken, deleteCookieOptions);
    }
}