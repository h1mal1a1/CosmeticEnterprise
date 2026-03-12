using CosmeticEnterpriseBack.Configuration;
using Microsoft.AspNetCore.Identity;
using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.Auth;
using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CosmeticEnterpriseBack.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly JwtSettings _jwtSettings;
    public AuthService(AppDbContext dbContext, ITokenService tokenService, IOptions<JwtSettings> jwtOptions)
    {
        _tokenService = tokenService;
        _dbContext = dbContext;
        _jwtSettings = jwtOptions.Value;
    }
    
    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username);
        if (existingUser != null)
            throw new Exception("User already exists");
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RoleName = "User",
            IsActive = true
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
        if (user == null)
            throw new Exception("Invalid username or password");
        if (!user.IsActive)
            throw new Exception("User is inactive");
        var passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        
        if(passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new Exception("Invalid username or password");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.ComputeSha256Hash(refreshToken);

        var refreshTokenEntity = new UserRefreshToken()
        {
            IdUser = user.IdUser,
            RefreshTokenHash = refreshTokenHash,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        _dbContext.UserRefreshTokens.Add(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request)
    {
        var refreshTokenHash = _tokenService.ComputeSha256Hash(request.RefreshToken);
        var existingRefreshToken = await _dbContext.UserRefreshTokens
            .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash);
        if (existingRefreshToken == null)
            throw new Exception("Refresh token is invalid");
        if(existingRefreshToken.IsRevoked)
            throw new Exception("Refresh token is expired");
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUser == existingRefreshToken.IdUser);
        if (user == null)
            throw new Exception("User not found");
        if(!user.IsActive)
            throw new Exception("User is inactive");
        existingRefreshToken.IsRevoked = true;
        existingRefreshToken.RevokedAtUtc = DateTime.UtcNow;

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshTokenHash = _tokenService.ComputeSha256Hash(newRefreshToken);

        var newRefreshTokenEntity = new UserRefreshToken
        {
            IdUser = user.IdUser,
            RefreshTokenHash = newRefreshTokenHash,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        _dbContext.UserRefreshTokens.Add(newRefreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<MeResponse> GetMeAsync(long idUser)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUser == idUser);
        if (user == null)
            throw new Exception("User not found");
        return new MeResponse
        {
            IdUser = user.IdUser,
            Username = user.Username,
            RoleName = user.RoleName
        };
    }
}