using CosmeticEnterpriseBack.DTO.FinishedProductImages;

namespace CosmeticEnterpriseBack.DTO.FinishedProduct;

public class FinishedProductResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long IdRecipe { get; set; }
    public long IdProductCategory { get; set; }
    public long IdUnitsOfMeasurement { get; set; }
    public List<FinishedProductImageResponse> Images { get; set; } = [];
}