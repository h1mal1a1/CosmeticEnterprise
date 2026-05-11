using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.Interfaces.Persistence;
using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.UseCases.FinishedProducts.UpdateStock;

public class UpdateFinishedProductStockUseCase(IFinishedProductStockRepository stockRepository) : IUpdateFinishedProductStockUseCase
{
    public async Task<FinishedProductStockResponse?> ExecuteAsync(long id, UpdateFinishedProductStockRequest request, CancellationToken cancellationToken)
    {
        var productExists = await stockRepository.FinishedProductExistsAsync(id, cancellationToken);

        if (!productExists)
            return null;

        var leftovers = await stockRepository.GetLeftoversAsync(id, cancellationToken);

        if (leftovers.Count == 0)
        {
            var warehouse = await stockRepository.GetOrCreateWarehouseAsync(cancellationToken);

            var leftover = new LeftoversInWarehouses
            {
                IdFinishedProduct = id,
                IdWarehouse = warehouse.Id,
                Quantity = request.AvailableQuantity,
                ReservedQuantity = 0
            };

            stockRepository.AddLeftover(leftover);
            leftovers.Add(leftover);

            await stockRepository.SaveChangesAsync(cancellationToken);

            return CreateStockResponse(id, leftovers);
        }

        SetAvailableQuantity(leftovers, request.AvailableQuantity);

        await stockRepository.SaveChangesAsync(cancellationToken);

        return CreateStockResponse(id, leftovers);
    }

    private static void SetAvailableQuantity(IReadOnlyList<LeftoversInWarehouses> leftovers, int requestedAvailableQuantity)
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
            var availableInCurrentLeftover = Math.Max(0, leftover.Quantity - leftover.ReservedQuantity);

            if (availableInCurrentLeftover == 0)
                continue;

            var quantityToRemoveFromCurrentLeftover = Math.Min(availableInCurrentLeftover, quantityToRemove);

            leftover.Quantity -= quantityToRemoveFromCurrentLeftover;
            quantityToRemove -= quantityToRemoveFromCurrentLeftover;

            if (quantityToRemove == 0)
                return;
        }
    }

    private static FinishedProductStockResponse CreateStockResponse(long id, IReadOnlyCollection<LeftoversInWarehouses> leftovers)
    {
        return new FinishedProductStockResponse
        {
            Id = id,
            Quantity = leftovers.Sum(x => x.Quantity),
            ReservedQuantity = leftovers.Sum(x => x.ReservedQuantity),
            AvailableQuantity = GetAvailableQuantity(leftovers)
        };
    }

    private static int GetAvailableQuantity(IEnumerable<LeftoversInWarehouses> leftovers) => 
        leftovers.Sum(x => Math.Max(0, x.Quantity - x.ReservedQuantity));
}