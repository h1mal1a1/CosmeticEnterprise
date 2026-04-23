namespace CosmeticEnterpriseBack.DTO.UserAddresses;

public class CreateUserAddressRequest
{
    public string RecipientName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string House { get; set; } = string.Empty;

    public string? Apartment { get; set; }

    public string? PostalCode { get; set; }

    public string? Comment { get; set; }

    public bool IsDefault { get; set; }
}