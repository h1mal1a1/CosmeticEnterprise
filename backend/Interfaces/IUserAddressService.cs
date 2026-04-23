using CosmeticEnterpriseBack.DTO.UserAddresses;

namespace CosmeticEnterpriseBack.Interfaces;

public interface IUserAddressService
{
    Task<IReadOnlyCollection<UserAddressResponse>> GetMyAddressesAsync(long userId, CancellationToken cancellationToken);

    Task<UserAddressResponse> GetMyAddressByIdAsync(long userId, long addressId, CancellationToken cancellationToken);

    Task<UserAddressResponse> CreateMyAddressAsync(long userId, CreateUserAddressRequest request, CancellationToken cancellationToken);

    Task<UserAddressResponse> UpdateMyAddressAsync(long userId, long addressId, UpdateUserAddressRequest request, CancellationToken cancellationToken);

    Task DeleteMyAddressAsync(long userId, long addressId, CancellationToken cancellationToken);

    Task<UserAddressResponse> SetDefaultAddressAsync(long userId, long addressId, CancellationToken cancellationToken);
}