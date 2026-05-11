using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
namespace CosmeticEnterpriseBack.Application.UseCases.FinishedProducts.UpdateStock;
public interface IUpdateFinishedProductStockUseCase
{
    Task<FinishedProductStockResponse?> ExecuteAsync(long id, UpdateFinishedProductStockRequest request, CancellationToken cancellationToken);
}