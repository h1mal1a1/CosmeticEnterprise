using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers.Base;

[ApiController]
public abstract class CrudController<TResponse, TCreateRequest, TUpdateRequest, TKey> : ControllerBase
{
    private readonly ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey> _crud;
    protected CrudController(ICrudService<TResponse, TCreateRequest, TUpdateRequest, TKey> crud) => _crud = crud;

    [HttpPost]
    public async Task<TResponse> Create(TCreateRequest request, CancellationToken cancellationToken) =>
        await _crud.CreateAsync(request, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ActionResult<TResponse>> Update(TKey id, TUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _crud.UpdateAsync(id, request, cancellationToken);
        return result == null ? NotFound() : result;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TResponse>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _crud.GetAllAsync(cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<TResponse>> GetById(TKey id, CancellationToken cancellationToken)
    {
        var result = await _crud.GetByIdAsync(id, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(TKey id, CancellationToken cancellationToken)
    {
        var deleted = await _crud.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    } 
}