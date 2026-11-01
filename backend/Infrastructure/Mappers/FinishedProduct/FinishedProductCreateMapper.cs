using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.FinishedProduct;

public class FinishedProductCreateMapper :
    ICreateMapper<FinishedProducts, CreateFinishedProductRequest>
{
    public FinishedProducts Map(CreateFinishedProductRequest request)
    {
        return new FinishedProducts
        {
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Price = request.Price,
            IdRecipe = request.IdRecipe,
            IdProductCategory = request.IdProductCategory,
            IdUnitsOfMeasurement = request.IdUnitsOfMeasurement
        };
    }
}