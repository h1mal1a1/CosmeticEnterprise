using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.FinishedProduct;

public class FinishedProductUpdateMapper :
    IUpdateMapper<Entities.FinishedProducts, UpdateFinishedProductRequest>
{
    public void Map(UpdateFinishedProductRequest req, Entities.FinishedProducts entity)
    {
        entity.Name = req.Name.Trim();
        entity.Price = req.Price;
        entity.IdRecipe = req.IdRecipe;
        entity.IdProductCategory = req.IdProductCategory;
        entity.IdUnitsOfMeasurement = req.IdUnitsOfMeasurement;
    }
}