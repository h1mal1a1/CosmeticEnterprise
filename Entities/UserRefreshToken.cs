namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Токены пользователей
/// </summary>
public class UserRefreshToken
{
    public long IdUserRefreshToken { get; set; }

    public long IdUser { get; set; }

    public string RefreshTokenHash { get; set; } = null!;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? RevokedAtUtc { get; set; }

    public bool IsRevoked { get; set; }

    public string? CreatedByIp { get; set; }

    public string? RevokedByIp { get; set; }

    public string? DeviceName { get; set; }

    public User User { get; set; } = null!;
}