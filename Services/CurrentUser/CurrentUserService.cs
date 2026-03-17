
using System.Security.Claims;
using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Services.Auth;

namespace CosmeticEnterpriseBack.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    

    public long? UserId
    {
        get
        {
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(claim, out var id) ? id : null;
        }
    }

    public string? Username => User?.FindFirst(ClaimTypes.Name)?.Value;

    public UserRole? Role 
    {
        get
        {
            var roleValue = User?.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(roleValue))
                return null;
            try
            {
                return RoleMapper.MapToEnum(roleValue);
            }
            catch
            {
                return null;
            }
        }
    }

    public bool IsInRole(UserRole role) => Role == role;
    public bool IsAdmin() => Role == UserRole.Admin;
    public bool IsManager() => Role == UserRole.Manager;
    public bool IsWarehouseManager() => Role == UserRole.WarehouseManager;
}