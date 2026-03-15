using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.FinishedProduct;

public class FinishedProductResponseMapper :
    IResponseMapper<Entities.FinishedProducts, FinishedProductResponse>
{
    public FinishedProductResponse Map(Entities.FinishedProducts entity)
    {
        return new FinishedProductResponse
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}