using System.Text.RegularExpressions;
using CosmeticEnterpriseBack.Application.DTOs.Users;
using CosmeticEnterpriseBack.Application.Exceptions;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Services.Users;

public class UserManagementService(AppDbContext dbContext, IUserMapper userMapper) : IUserManagementService
{
    private static readonly Regex PhoneRegex = new(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled);

    private readonly AppDbContext _dbContext = dbContext;
    private readonly IUserMapper _userMapper = userMapper;

    public async Task<IReadOnlyList<UserResponse>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .OrderBy(x => x.IdUser)
            .ToListAsync(cancellationToken);

        return [.. users.Select(_userMapper.MapToResponse)];
    }

    public async Task<UserResponse> GetUserByIdAsync(long idUser, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdUser == idUser, cancellationToken);

        if (user is null)
            throw new NotFoundException("User not found");

        return _userMapper.MapToResponse(user);
    }

    public async Task<UserResponse> UpdateUserAsync(long idUser, UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var email = request.Email.Trim();
        var phone = request.Phone.Trim();

        ValidateRequest(username, email, phone);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.IdUser == idUser, cancellationToken);

        if (user is null)
            throw new NotFoundException("User not found");

        var usernameExists = await _dbContext.Users
            .AnyAsync(x => x.IdUser != idUser && x.Username == username, cancellationToken);

        if (usernameExists)
            throw new ConflictException("Username already exists");

        var emailExists = await _dbContext.Users
            .AnyAsync(x => x.IdUser != idUser && x.Email == email, cancellationToken);

        if (emailExists)
            throw new ConflictException("Email already exists");

        var phoneExists = await _dbContext.Users
            .AnyAsync(x => x.IdUser != idUser && x.Phone == phone, cancellationToken);

        if (phoneExists)
            throw new ConflictException("Phone already exists");

        await EnsureAtLeastOneActiveAdminRemainsAsync(user, request.RoleName, request.IsActive, cancellationToken);

        user.Username = username;
        user.Email = email;
        user.Phone = phone;
        user.RoleName = request.RoleName;
        user.IsActive = request.IsActive;
        user.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _userMapper.MapToResponse(user);
    }

    private async Task EnsureAtLeastOneActiveAdminRemainsAsync(User currentUser, UserRole newRole,
        bool newIsActive, CancellationToken cancellationToken)
    {
        var removesAdminAccess =
            currentUser.RoleName == UserRole.Admin &&
            (newRole != UserRole.Admin || !newIsActive);

        if (!removesAdminAccess)
            return;

        var hasAnotherActiveAdmin = await _dbContext.Users
            .AnyAsync(
                x =>
                    x.IdUser != currentUser.IdUser &&
                    x.RoleName == UserRole.Admin &&
                    x.IsActive,
                cancellationToken);

        if (!hasAnotherActiveAdmin)
            throw new BadRequestException("At least one active admin must remain");
    }

    private static void ValidateRequest(string username, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BadRequestException("Username is required");

        if (username.Length > 100)
            throw new BadRequestException("Username is too long");

        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("Email is required");

        if (!email.Contains('@') || email.Length > 255)
            throw new BadRequestException("Invalid email");

        if (string.IsNullOrWhiteSpace(phone))
            throw new BadRequestException("Phone is required");

        if (!PhoneRegex.IsMatch(phone))
            throw new BadRequestException("Phone must be in format +123456789");
    }
}