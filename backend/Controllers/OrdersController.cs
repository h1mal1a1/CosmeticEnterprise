using System.Security.Claims;
using CosmeticEnterpriseBack.DTO.Orders;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderListItemResponse>>> GetMyOrders([FromQuery] GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await orderService.GetMyOrdersAsync(userId, query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderResponse>> GetMyOrderById(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await orderService.GetMyOrderByIdAsync(userId, id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderResponse>> Checkout([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await orderService.CreateOrderFromCartAsync(userId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<ActionResult<OrderResponse>> CancelMyOrder(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await orderService.CancelMyOrderAsync(userId, id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<PagedResult<OrderListItemResponse>>> GetAllOrders([FromQuery] GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var result = await orderService.GetAllOrdersAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("admin/{id:long}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(long id, CancellationToken cancellationToken)
    {
        var result = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("admin/{id:long}/statuses")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponse>> UpdateStatuses(long id, [FromBody] UpdateOrderStatusesRequest request, CancellationToken cancellationToken)
    {
        var result = await orderService.UpdateOrderStatusesAsync(id, request, cancellationToken);
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