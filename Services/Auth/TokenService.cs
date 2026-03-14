using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CosmeticEnterpriseBack.Services.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings) => _jwtSettings = jwtSettings.Value;
    
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.RoleName),
            new("token_type", "access")
        };
        return GenerateJwtToken(claims, DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeMinutes));
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("token_type", "refresh")
        };
        return GenerateJwtToken(claims, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays));
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),

            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out var validatedToken);
            return validatedToken is not JwtSecurityToken jwtToken ? null : principal;
        }
        catch
        {
            return null;    
        }
        
    }

    private string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiresUtc)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresUtc,
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}