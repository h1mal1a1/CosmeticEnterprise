using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Exceptions;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Services.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationService(ICurrentUserService currentUserService) => _currentUserService = currentUserService;

    public bool HasAccess(ResourceType resource, CrudAction crudAction)
    {
        if (!_currentUserService.IsAuthenticated)
            return false;
        if (_currentUserService.Role is null)
            return false;
        return RolePermissionMatrix.HasAccess(_currentUserService.Role.Value, resource, crudAction);
    }

    public void EnsureAccess(ResourceType resource, CrudAction action)
    {
        if (!HasAccess(resource, action))
            throw new UnauthorizedAccessException($"Access denied. Resourse: {resource}, action: {action}");
    }
}