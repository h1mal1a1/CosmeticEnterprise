namespace CosmeticEnterpriseBack.Api.DTOs.FinishedProduct;
public class FinishedProductStockResponse
{
    public long Id { get; set; }

    public int Quantity { get; set; }

    public int ReservedQuantity { get; set; }

    public int AvailableQuantity { get; set; }
}