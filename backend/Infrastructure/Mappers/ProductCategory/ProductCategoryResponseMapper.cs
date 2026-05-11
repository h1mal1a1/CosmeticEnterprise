using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.ProductCategory;

public class ProductCategoryResponseMapper:
    IResponseMapper<Domain.Entities.ProductCategories, ProductCategoryResponse>
{
    public ProductCategoryResponse Map(Domain.Entities.ProductCategories entity) => 
        new() { Id = entity.Id, Name = entity.Name };
}