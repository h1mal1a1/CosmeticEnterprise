using CosmeticEnterpriseBack.Application.DTOs.Users;

namespace CosmeticEnterpriseBack.Application.Interfaces;

public interface IUserManagementService
{
    Task<IReadOnlyList<UserResponse>> GetUsersAsync(CancellationToken cancellationToken);

    Task<UserResponse> GetUserByIdAsync(long idUser, CancellationToken cancellationToken);

    Task<UserResponse> UpdateUserAsync(
        long idUser,
        UpdateUserRequest request,
        CancellationToken cancellationToken);
}