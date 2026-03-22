using CosmeticEnterpriseBack.Authorization;

namespace CosmeticEnterpriseBack.Services.Auth;

public static class RoleMapper
{
    public static UserRole MapToEnum(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is empty");
        
        return role.Trim().ToLowerInvariant() switch
        {
            "admin" => UserRole.Admin,
            "manager" => UserRole.Manager,
            "warehousemanager" => UserRole.WarehouseManager,
            "user" => UserRole.User,
            _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role: {role}")
        };
    }
}