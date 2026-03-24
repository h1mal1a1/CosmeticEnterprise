using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.ProductCategory;

public class ProductCategoryResponseMapper:
    IResponseMapper<Entities.ProductCategories, ProductCategoryResponse>
{
    public ProductCategoryResponse Map(Entities.ProductCategories entity) => 
        new() { Id = entity.Id, Name = entity.Name };
}