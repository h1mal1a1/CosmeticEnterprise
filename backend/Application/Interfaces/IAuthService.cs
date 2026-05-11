using CosmeticEnterpriseBack.Application.DTOs.Auth;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RefreshAsync(string refreshToken);

    Task<MeResponse> GetMeAsync(long idUser);

    Task<MeResponse> UpdateProfileAsync(long idUser, UpdateProfileRequest request);
}