using CosmeticEnterpriseBack.Application.DTOs.UserAddresses;

namespace CosmeticEnterpriseBack.Application.Validators;

public class UserAddressValidator : IUserAddressValidator
{
    public void ValidateCreate(CreateUserAddressRequest request)
    {
        CheckRequired(request.RecipientName, nameof(request.RecipientName));
        CheckRequired(request.Phone, nameof(request.Phone));
        CheckRequired(request.Country, nameof(request.Country));
        CheckRequired(request.City, nameof(request.City));
        CheckRequired(request.Street, nameof(request.Street));
        CheckRequired(request.House, nameof(request.House));
    }

    public void ValidateUpdate(UpdateUserAddressRequest request)
    {
        CheckRequired(request.RecipientName, nameof(request.RecipientName));
        CheckRequired(request.Phone, nameof(request.Phone));
        CheckRequired(request.Country, nameof(request.Country));
        CheckRequired(request.City, nameof(request.City));
        CheckRequired(request.Street, nameof(request.Street));
        CheckRequired(request.House, nameof(request.House));
    }

    private static void CheckRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} is required.");
    }
}