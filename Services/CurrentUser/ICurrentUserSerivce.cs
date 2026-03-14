namespace CosmeticEnterpriseBack.Services.CurrentUser;

public interface ICurrentUserSerivce
{
    long? IdUser { get; }
    string? Username { get; }
    string? RoleName { get; }
    bool IsAuthenticated { get; }
}