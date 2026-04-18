using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.FinishedProduct;

public class FinishedProductCreateMapper :
    ICreateMapper<FinishedProducts, CreateFinishedProductRequest>
{
    public FinishedProducts Map(CreateFinishedProductRequest request)
    {
        return new FinishedProducts
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            IdRecipe = request.IdRecipe,
            IdProductCategory = request.IdProductCategory,
            IdUnitsOfMeasurement = request.IdUnitsOfMeasurement
        };
    }
}