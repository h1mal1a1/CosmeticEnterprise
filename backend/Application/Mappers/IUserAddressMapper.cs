using CosmeticEnterpriseBack.Api.DTOs.UserAddresses;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

/// <summary>
/// Контракт маппинга между доменной сущностью адреса и DTO.
/// </summary>
public interface IUserAddressMapper
{
    UserAddressResponse ToResponse(UserAddress address);
    IReadOnlyCollection<UserAddressResponse> ToResponseList(IReadOnlyCollection<UserAddress> addresses);
}