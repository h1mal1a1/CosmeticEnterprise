using System.Security.Claims;
using CosmeticEnterpriseBack.DTO.Cart;
using CosmeticEnterpriseBack.Services.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Authorize]
[Route("api/cart")]
public sealed class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ShoppingCartResponse>> GetCart(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var response = await cartService.GetCartAsync(userId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("items")]
    public async Task<ActionResult<ShoppingCartResponse>> AddItem([FromBody] AddCartItemRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var response = await cartService.AddItemAsync(userId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPut("items/{itemId:long}")]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateItemQuantity([FromRoute] long itemId,
        [FromBody] UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var response = await cartService.UpdateItemQuantityAsync(userId, itemId, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("items/{itemId:long}")]
    public async Task<ActionResult<ShoppingCartResponse>> RemoveItem([FromRoute] long itemId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var response = await cartService.RemoveItemAsync(userId, itemId, cancellationToken);
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        await cartService.ClearCartAsync(userId, cancellationToken);
        return NoContent();
    }
    private long GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out var userId))
            throw new UnauthorizedAccessException("User id claim was not found.");
        return userId;
    }
}