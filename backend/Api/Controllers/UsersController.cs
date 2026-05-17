using CosmeticEnterpriseBack.Application.DTOs.Users;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/users")]
public class UsersController(IUserManagementService userManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await userManagementService.GetUsersAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserResponse>> GetUserById(long id, CancellationToken cancellationToken)
    {
        var result = await userManagementService.GetUserByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(long id, [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userManagementService.UpdateUserAsync(id, request, cancellationToken);
        return Ok(result);
    }
}