using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.ProductCategory;

public class ProductCategoryUpdateMapper : 
    IUpdateMapper<ProductCategories, UpdateProductCategoryRequest>
{
    public void Map(UpdateProductCategoryRequest request, ProductCategories entity)
    {
        entity.Name = request.Name;
    }
}