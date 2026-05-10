using CosmeticEnterpriseBack.Infrastructure.Authorization;

namespace CosmeticEnterpriseBack.Infrastructure.Interfaces;

public interface IAuthorizationService
{
    bool HasAccess(ResourceType resource, CrudAction action);
    void EnsureAccess(ResourceType resource, CrudAction action);
}