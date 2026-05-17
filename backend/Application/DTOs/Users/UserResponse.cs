using CosmeticEnterpriseBack.Domain.Enums;

namespace CosmeticEnterpriseBack.Application.DTOs.Users;

public class UserResponse
{
    public long Id { get; set; }

    public long IdUser { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public UserRole RoleName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}