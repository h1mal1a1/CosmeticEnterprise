using System.Security.Claims;
using System.Text.RegularExpressions;
using CosmeticEnterpriseBack.Authorization;
using Microsoft.AspNetCore.Identity;
using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.Auth;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(AppDbContext dbContext, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BadRequestException("Email is required");

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new BadRequestException("Phone is required");

        // E.164 validation
        var phoneRegex = new Regex(@"^\+[1-9]\d{7,14}$");
        if (!phoneRegex.IsMatch(request.Phone))
            throw new BadRequestException("Phone must be in format +123456789");

        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(x =>
                x.Username == request.Username ||
                x.Email == request.Email ||
                x.Phone == request.Phone);

        if (existingUser != null)
            throw new ConflictException("User already exists");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RoleName = UserRole.User,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null)
            throw new UnauthorizedException("Invalid username or password");

        if (!user.IsActive)
            throw new UnauthorizedException("User is inactive");

        var passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new UnauthorizedException("Invalid username or password");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken)
    {
        var principal = _tokenService.GetPrincipalFromToken(refreshToken, validateLifetime: true);

        if (principal == null)
            throw new UnauthorizedException("Invalid refresh token");

        var tokenType = principal.FindFirst("token_type")?.Value;
        if (tokenType != "refresh")
            throw new UnauthorizedException("Invalid token type");

        var idUserClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(idUserClaim) || !long.TryParse(idUserClaim, out var idUser))
            throw new UnauthorizedException("Invalid refresh token payload");

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.IdUser == idUser);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!user.IsActive)
            throw new UnauthorizedException("User is inactive");

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<MeResponse> GetMeAsync(long idUser)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.IdUser == idUser);

        if (user == null)
            throw new NotFoundException("User not found");

        return new MeResponse
        {
            IdUser = user.IdUser,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName.ToString()
        };
    }
}