namespace CosmeticEnterpriseBack.DTO.FinishedProduct;

public class FinishedProductResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long IdRecipe { get; set; }
    public long IdProductCategory { get; set; }
    public long IdUnitsOfMeasurement { get; set; }
}