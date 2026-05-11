using CosmeticEnterpriseBack.Application.DTOs.ProductCategory;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Infrastructure.Interfaces;

namespace CosmeticEnterpriseBack.Infrastructure.Mappers.ProductCategory;

public class ProductCategoryCreateMapper : ICreateMapper<ProductCategories, CreateProductCategoryRequest>
{
    public ProductCategories Map(CreateProductCategoryRequest request) => new() { Name = request.Name };
}