using CosmeticEnterpriseBack.Domain.Enums;

namespace CosmeticEnterpriseBack.Application.DTOs.Users;

public class UpdateUserRequest
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public UserRole RoleName { get; set; }

    public bool IsActive { get; set; }
}