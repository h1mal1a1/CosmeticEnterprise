using CosmeticEnterpriseBack.Api.DTOs.UserAddresses;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

public class UserAddressMapper : IUserAddressMapper
{
    public UserAddressResponse ToResponse(UserAddress address)
    {
        return new UserAddressResponse
        {
            Id = address.Id,
            IdUser = address.IdUser,
            RecipientName = address.RecipientName,
            Phone = address.Phone,
            Country = address.Country,
            City = address.City,
            Street = address.Street,
            House = address.House,
            Apartment = address.Apartment,
            PostalCode = address.PostalCode,
            Comment = address.Comment,
            IsDefault = address.IsDefault,
            CreatedAtUtc = address.CreatedAtUtc,
            UpdatedAtUtc = address.UpdatedAtUtc
        };
    }

    public IReadOnlyCollection<UserAddressResponse> ToResponseList(IReadOnlyCollection<UserAddress> addresses)
    {
        return [.. addresses.Select(ToResponse)];
    }
}