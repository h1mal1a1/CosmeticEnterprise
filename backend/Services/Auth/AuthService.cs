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
    private static readonly Regex PhoneRegex = new(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled);

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
        ValidateEmailAndPhone(request.Email, request.Phone);

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
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
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

        return MapToMeResponse(user);
    }

    public async Task<MeResponse> UpdateProfileAsync(long idUser, UpdateProfileRequest request)
    {
        ValidateEmailAndPhone(request.Email, request.Phone);

        var email = request.Email.Trim();
        var phone = request.Phone.Trim();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.IdUser == idUser);

        if (user == null)
            throw new NotFoundException("User not found");

        var isEmailBusy = await _dbContext.Users
            .AnyAsync(x => x.IdUser != idUser && x.Email == email);

        if (isEmailBusy)
            throw new ConflictException("Email already exists");

        var isPhoneBusy = await _dbContext.Users
            .AnyAsync(x => x.IdUser != idUser && x.Phone == phone);

        if (isPhoneBusy)
            throw new ConflictException("Phone already exists");

        user.Email = email;
        user.Phone = phone;
        user.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return MapToMeResponse(user);
    }

    private static void ValidateEmailAndPhone(string? email, string? phone)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("Email is required");

        if (string.IsNullOrWhiteSpace(phone))
            throw new BadRequestException("Phone is required");

        if (!email.Contains('@') || email.Length > 255)
            throw new BadRequestException("Invalid email");

        if (!PhoneRegex.IsMatch(phone))
            throw new BadRequestException("Phone must be in format +123456789");
    }

    private static MeResponse MapToMeResponse(User user)
    {
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