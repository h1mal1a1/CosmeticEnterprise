using System.ComponentModel.DataAnnotations;

namespace CosmeticEnterpriseBack.DTO.FinishedProduct;

public class UpdateFinishedProductRequest
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    [Range(0.01, 999999999)]
    public decimal Price { get; set; }
    public long IdRecipe { get; set; }
    public long IdProductCategory { get; set; }
    public long IdUnitsOfMeasurement { get; set; }
}