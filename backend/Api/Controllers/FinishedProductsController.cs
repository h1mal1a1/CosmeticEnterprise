using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Api.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.Authorization;
using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.UseCases.FinishedProducts.UpdateStock;
using CosmeticEnterpriseBack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Route("api/finished-products")]
public class FinishedProductsController(ICrudServiceFactory crudFactory, IUpdateFinishedProductStockUseCase updateStockUseCase)
    : CrudController<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long>
(
    crudFactory.Create
        <FinishedProducts, long, CreateFinishedProductRequest, UpdateFinishedProductRequest, FinishedProductResponse>
        (ResourceType.FinishedProduct)
)
{
    [HttpPut("{id:long}/stock")]
    public async Task<ActionResult<FinishedProductStockResponse>> UpdateStock(long id, UpdateFinishedProductStockRequest request, 
    CancellationToken cancellationToken)
    {
        var response = await updateStockUseCase.ExecuteAsync(id, request, cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}