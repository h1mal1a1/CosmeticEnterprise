using System.ComponentModel.DataAnnotations;

namespace CosmeticEnterpriseBack.DTO.Auth;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;
}