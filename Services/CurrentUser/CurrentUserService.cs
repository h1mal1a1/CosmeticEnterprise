
using System.Security.Claims;

namespace CosmeticEnterpriseBack.Services.CurrentUser;

public class CurrentUserService : ICurrentUserSerivce
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public long? IdUser
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

    public string? RoleName =>
            _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Role)
            .Value;

}