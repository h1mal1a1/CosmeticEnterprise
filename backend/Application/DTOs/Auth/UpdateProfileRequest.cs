namespace CosmeticEnterpriseBack.Application.DTOs.Auth;
public class UpdateProfileRequest
{
    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;
}