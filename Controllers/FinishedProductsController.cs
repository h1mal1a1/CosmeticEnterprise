using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Services.CurrentUser;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

[ApiController]
[Route("api/finished-products")]
public class FinishedProductsController : ControllerBase
{
    private readonly ICrudService<
        FinishedProductResponse,
        CreateFinishedProductRequest,
        UpdateFinishedProductRequest,
        long> _crud;

    public FinishedProductsController(
        ICrudService<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long> crud) =>
        _crud = crud;

    [HttpPost]
    public async Task<FinishedProductResponse> Create(CreateFinishedProductRequest req,
        CancellationToken cancellationToken) => await _crud.CreateAsync(req, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ActionResult<FinishedProductResponse>> Update(long id, UpdateFinishedProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _crud.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : result;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var deleted = await _crud.DeleteAsync(id, cancellationToken);
        return !deleted ? NotFound() : NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FinishedProductResponse>>> GetAll(
        CancellationToken cancellationToken) => Ok(await _crud.GetAllAsync(cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<FinishedProductResponse>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _crud.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}