using CosmeticEnterpriseBack.Api.DTOs.UserAddresses;

namespace CosmeticEnterpriseBack.Application.Validators;

/// <summary>
/// Контракт валидации входных запросов для сценариев работы с адресами.
/// </summary>
public interface IUserAddressValidator
{
    void ValidateCreate(CreateUserAddressRequest request);
    void ValidateUpdate(UpdateUserAddressRequest request);
}