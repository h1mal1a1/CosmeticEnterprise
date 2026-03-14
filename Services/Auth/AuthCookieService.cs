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
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeMinutes)
        };
        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays)
        };
        
        response.Cookies.Append(AuthCookieNames.AccessToken, authResponse.AccessToken,accessCookieOptions);
        response.Cookies.Append(AuthCookieNames.RefreshToken, authResponse.RefreshToken,refreshCookieOptions);
    }

    public void DeleteAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete(AuthCookieNames.AccessToken);
        response.Cookies.Delete(AuthCookieNames.RefreshToken);
    }
}