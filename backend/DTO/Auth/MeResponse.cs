namespace CosmeticEnterpriseBack.DTO.Auth;

public class MeResponse
{
    public long IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string RoleName { get; set; } = null!;
}