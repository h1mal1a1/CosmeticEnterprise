using CosmeticEnterpriseBack.Application.DTOs.Users;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Mappers;

public interface IUserMapper
{
    UserResponse MapToResponse(User user);
}