using CosmeticEnterpriseBack.Application.DTOs.Users;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

public class UserMapper : IUserMapper
{
    public UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.IdUser,
            IdUser = user.IdUser,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName,
            IsActive = user.IsActive,
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc
        };
    }
}