using CosmeticEnterpriseBack.Authorization;

namespace CosmeticEnterpriseBack.Interfaces;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? Username { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(UserRole role);
    bool IsAdmin();
    bool IsManager();
    bool IsWarehouseManager();
}