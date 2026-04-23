using CosmeticEnterpriseBack.DTO.Orders;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Route("api/order-dictionaries")]
[Authorize]
public class OrderDictionariesController(IOrderDictionaryService orderDictionaryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<OrderDictionariesResponse>> Get(CancellationToken cancellationToken)
    {
        var result = await orderDictionaryService.GetOrderDictionariesAsync(cancellationToken);
        return Ok(result);
    }
}