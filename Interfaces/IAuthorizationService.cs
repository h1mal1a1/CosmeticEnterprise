using CosmeticEnterpriseBack.Authorization;

namespace CosmeticEnterpriseBack.Interfaces;

public interface IAuthorizationService
{
    bool HasAccess(ResourceType resource, CrudAction action);
    void EnsureAccess(ResourceType resource, CrudAction action);
}