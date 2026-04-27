using CosmeticEnterpriseBack.Authorization;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Пользователи
/// </summary>
public class User
{
    public long IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public UserRole RoleName { get; set; } = UserRole.User;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<UserRefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserAddress> UserAddresses { get; set; } = [];
    public ICollection<Orders> Orders { get; set; } = [];
}