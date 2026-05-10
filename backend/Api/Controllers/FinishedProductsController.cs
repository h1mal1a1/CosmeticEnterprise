using CosmeticEnterpriseBack.Infrastructure.Authorization;
using CosmeticEnterpriseBack.Api.Controllers.Base;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using CosmeticEnterpriseBack.Api.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Api.Controllers;

[ApiController]
[Route("api/finished-products")]
public class FinishedProductsController(ICrudServiceFactory crudFactory, AppDbContext dbContext) 
: CrudController<FinishedProductResponse, CreateFinishedProductRequest, UpdateFinishedProductRequest, long>
(crudFactory.Create<FinishedProducts,long,CreateFinishedProductRequest,UpdateFinishedProductRequest,FinishedProductResponse>(ResourceType.FinishedProduct))
{
    private readonly AppDbContext _dbContext = dbContext;

    [HttpPut("{id:long}/stock")]
    public async Task<ActionResult<FinishedProductStockResponse>> UpdateStock(
        long id,
        UpdateFinishedProductStockRequest request,
        CancellationToken cancellationToken)
    {
        var productExists = await _dbContext.FinishedProducts
            .AnyAsync(x => x.Id == id, cancellationToken);

        if (!productExists)
            return NotFound();

        var leftovers = await _dbContext.LeftoversInWarehouses
            .Where(x => x.IdFinishedProduct == id)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        if (leftovers.Count == 0)
        {
            var warehouse = await GetOrCreateWarehouseAsync(cancellationToken);

            var leftover = new LeftoversInWarehouses
            {
                IdFinishedProduct = id,
                IdWarehouse = warehouse.Id,
                Quantity = request.AvailableQuantity,
                ReservedQuantity = 0
            };

            _dbContext.LeftoversInWarehouses.Add(leftover);
            leftovers.Add(leftover);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(CreateStockResponse(id, leftovers));
        }

        SetAvailableQuantity(leftovers, request.AvailableQuantity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(CreateStockResponse(id, leftovers));
    }

    private async Task<Warehouses> GetOrCreateWarehouseAsync(CancellationToken cancellationToken)
    {
        var warehouse = await _dbContext.Warehouses
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (warehouse is not null)
            return warehouse;

        warehouse = new Warehouses();

        _dbContext.Warehouses.Add(warehouse);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return warehouse;
    }

    private static void SetAvailableQuantity(
        IReadOnlyList<LeftoversInWarehouses> leftovers,
        int requestedAvailableQuantity)
    {
        var currentAvailableQuantity = GetAvailableQuantity(leftovers);
        var difference = requestedAvailableQuantity - currentAvailableQuantity;

        if (difference > 0)
        {
            leftovers[0].Quantity += difference;
            return;
        }

        if (difference == 0)
            return;

        var quantityToRemove = Math.Abs(difference);

        foreach (var leftover in leftovers)
        {
            var availableInCurrentLeftover = Math.Max(
                0,
                leftover.Quantity - leftover.ReservedQuantity);

            if (availableInCurrentLeftover == 0)
                continue;

            var quantityToRemoveFromCurrentLeftover = Math.Min(
                availableInCurrentLeftover,
                quantityToRemove);

            leftover.Quantity -= quantityToRemoveFromCurrentLeftover;
            quantityToRemove -= quantityToRemoveFromCurrentLeftover;

            if (quantityToRemove == 0)
                return;
        }
    }

    private static FinishedProductStockResponse CreateStockResponse(
        long id,
        IReadOnlyCollection<LeftoversInWarehouses> leftovers)
    {
        return new FinishedProductStockResponse
        {
            Id = id,
            Quantity = leftovers.Sum(x => x.Quantity),
            ReservedQuantity = leftovers.Sum(x => x.ReservedQuantity),
            AvailableQuantity = GetAvailableQuantity(leftovers)
        };
    }

    private static int GetAvailableQuantity(IEnumerable<LeftoversInWarehouses> leftovers)
    {
        return leftovers.Sum(x => Math.Max(0, x.Quantity - x.ReservedQuantity));
    }
}