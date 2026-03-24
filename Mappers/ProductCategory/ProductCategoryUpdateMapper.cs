using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.ProductCategory;

public class ProductCategoryUpdateMapper : 
    IUpdateMapper<ProductCategories, UpdateProductCategoryRequest>
{
    public void Map(UpdateProductCategoryRequest request, ProductCategories entity)
    {
        entity.Name = request.Name;
    }
}