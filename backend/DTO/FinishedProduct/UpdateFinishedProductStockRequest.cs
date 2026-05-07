using System.ComponentModel.DataAnnotations;

namespace CosmeticEnterpriseBack.DTO.FinishedProduct;

public class UpdateFinishedProductStockRequest
{
    [Range(0, int.MaxValue)]
    public int AvailableQuantity { get; set; }
}