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
            return IsPublicRead(resource, crudAction);
            
        if (_currentUserService.Role is null)
            return false;
        return RolePermissionMatrix.HasAccess(_currentUserService.Role.Value, resource, crudAction);
    }

    public void EnsureAccess(ResourceType resource, CrudAction action)
    {
        if (!HasAccess(resource, action))
            throw new ForbiddenException($"Access denied. Resource: {resource}, action: {action}");
    }

    private static bool IsPublicRead(ResourceType resource, CrudAction action)
    {
        if (action != CrudAction.Read) return false;
        return resource is ResourceType.ProductCategory or ResourceType.FinishedProduct;
    }
}