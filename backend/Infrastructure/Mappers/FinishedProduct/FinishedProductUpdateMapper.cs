using CosmeticEnterpriseBack.Application.DTOs.FinishedProduct;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.FinishedProduct;

public class FinishedProductUpdateMapper :
    IUpdateMapper<Domain.Entities.FinishedProducts, UpdateFinishedProductRequest>
{
    public void Map(UpdateFinishedProductRequest req, Domain.Entities.FinishedProducts entity)
    {
        entity.Name = req.Name.Trim();
        entity.Price = req.Price;
        entity.IdRecipe = req.IdRecipe;
        entity.IdProductCategory = req.IdProductCategory;
        entity.IdUnitsOfMeasurement = req.IdUnitsOfMeasurement;
    }
}