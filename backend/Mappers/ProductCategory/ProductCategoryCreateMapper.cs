using CosmeticEnterpriseBack.DTO.ProductCategory;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Mappers.ProductCategory;

public class ProductCategoryCreateMapper : ICreateMapper<ProductCategories, CreateProductCategoryRequest>
{
    public ProductCategories Map(CreateProductCategoryRequest request) => new() { Name = request.Name };
}