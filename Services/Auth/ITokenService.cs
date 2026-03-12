using CosmeticEnterpriseBack.Entities;

namespace CosmeticEnterpriseBack.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string ComputeSha256Hash(string value);
}