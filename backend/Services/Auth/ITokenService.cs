using System.Security.Claims;
using CosmeticEnterpriseBack.Entities;

namespace CosmeticEnterpriseBack.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime);
}