using System.Security.Claims;
using CosmeticEnterpriseBack.DTO.UserAddresses;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Route("api/user-addresses")]
[Authorize]
public class UserAddressesController(IUserAddressService userAddressService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UserAddressResponse>>> GetMyAddresses(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await userAddressService.GetMyAddressesAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserAddressResponse>> GetMyAddressById(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await userAddressService.GetMyAddressByIdAsync(userId, id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserAddressResponse>> CreateMyAddress([FromBody] CreateUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await userAddressService.CreateMyAddressAsync(userId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<UserAddressResponse>> UpdateMyAddress(long id, [FromBody] UpdateUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await userAddressService.UpdateMyAddressAsync(userId, id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteMyAddress(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        await userAddressService.DeleteMyAddressAsync(userId, id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:long}/set-default")]
    public async Task<ActionResult<UserAddressResponse>> SetDefaultAddress(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await userAddressService.SetDefaultAddressAsync(userId, id, cancellationToken);
        return Ok(result);
    }

    private long GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub")
                    ?? User.FindFirstValue("id");

        if (string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out var userId))
            throw new UnauthorizedAccessException("User id claim not found.");

        return userId;
    }
}