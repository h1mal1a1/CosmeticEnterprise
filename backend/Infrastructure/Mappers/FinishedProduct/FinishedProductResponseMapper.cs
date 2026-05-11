using CosmeticEnterpriseBack.Api.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Application.DTOs.FinishedProductImages;
using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.FinishedProduct;

public class FinishedProductResponseMapper :
    IResponseMapper<Domain.Entities.FinishedProducts, FinishedProductResponse>
{
    private readonly IObjectStorageService _objectStorageService;

    public FinishedProductResponseMapper(IObjectStorageService objectStorageService) =>
        _objectStorageService = objectStorageService;

    public FinishedProductResponse Map(Domain.Entities.FinishedProducts entity)
    {
        return new FinishedProductResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            WbUrl = entity.WbUrl,
            IdRecipe = entity.IdRecipe,
            IdProductCategory = entity.IdProductCategory,
            IdUnitsOfMeasurement = entity.IdUnitsOfMeasurement,
            AvailableQuantity = entity.LeftoversInWarehousesList
                .Sum(x => Math.Max(0, x.Quantity - x.ReservedQuantity)),
            Images = entity.Images
                .OrderBy(x => x.SortOrder)
                .Select(x => new FinishedProductImageResponse
                {
                    Id = x.Id,
                    FileUrl = _objectStorageService.GetFileUrl(x.ObjectKey),
                    SortOrder = x.SortOrder,
                    IsMain = x.IsMain
                })
                .ToList()
        };
    }
}