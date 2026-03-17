namespace CosmeticEnterpriseBack.Authorization;

public class RolePermissionMatrix
{
    public static bool HasAccess(UserRole role, ResourceType resource, CrudAction action)
    {
        return role switch
        {
            UserRole.Admin => true,
            UserRole.Manager => HasManagerAccess(resource, action),
            UserRole.WarehouseManager => HasWarehouseAccess(resource ,action),
            UserRole.User => HasUserAccess(resource, action),
            _ => false
        };
    }

    private static bool HasManagerAccess(ResourceType resource, CrudAction action)
    {
        return resource switch
        {
            ResourceType.FinishedProduct => action is CrudAction.Create or CrudAction.Read or CrudAction.Update,
            ResourceType.ProductCategory => action is CrudAction.Create or CrudAction.Read or CrudAction.Update,
            ResourceType.Recipe => action is CrudAction.Create or CrudAction.Read or CrudAction.Update,
            ResourceType.Material => action is CrudAction.Read,
            ResourceType.UnitOfMeasurement => action is CrudAction.Read,
            ResourceType.Order => action is CrudAction.Read or CrudAction.Update,
            ResourceType.Customer => action is CrudAction.Read,
            _ => false
        };
    }
    private static bool HasWarehouseAccess(ResourceType resource, CrudAction action)
    {
        return resource switch
        {
            ResourceType.Material => action is CrudAction.Create or CrudAction.Read or CrudAction.Update,
            ResourceType.Warehouse => action is CrudAction.Read or CrudAction.Update,
            ResourceType.ProductCategory => action is CrudAction.Create or CrudAction.Read or CrudAction.Update,
            ResourceType.UnitOfMeasurement => action is CrudAction.Read,
            ResourceType.FinishedProduct => action is CrudAction.Read,
            _ => false
        };
    }
    private static bool HasUserAccess(ResourceType resource, CrudAction action)
    {
        return resource switch
        {
            ResourceType.FinishedProduct => action is CrudAction.Read,
            ResourceType.ProductCategory => action is CrudAction.Read,
            _ => false
        };
    }
}