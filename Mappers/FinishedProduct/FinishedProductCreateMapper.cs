using CosmeticEnterpriseBack.DTO.FinishedProduct;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.FinishedProduct;

public class FinishedProductCreateMapper :
    ICreateMapper<Entities.FinishedProducts, CreateFinishedProductRequest>
{
    public Entities.FinishedProducts Map(CreateFinishedProductRequest request)
    {
        return new Entities.FinishedProducts() { Name = request.Name.Trim() };
    }
}