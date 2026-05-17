using System.Security.Claims;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Domain.Enums;
using CosmeticEnterpriseBack.Infrastructure.Services.Auth;

namespace CosmeticEnterpriseBack.Api.Services.CurrentUser;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

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