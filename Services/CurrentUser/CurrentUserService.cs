
using System.Security.Claims;
using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public long? UserId
    {
        get
        {
            var claim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;
            if (long.TryParse(claim, out var id))
                return id;
            return null;
        }
    }

    public string? Username =>
            _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Name)?
            .Value;

    public UserRole? Role 
    {
        get
        {
            var roleValue = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Role)?
                .Value;
            if (string.IsNullOrWhiteSpace(roleValue))
                return null;
            if (Enum.TryParse<UserRole>(roleValue, ignoreCase: true, out var role))
                return role;
            return null;
        }
    }
}