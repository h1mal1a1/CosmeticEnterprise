namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Производственные партии
/// </summary>
public class ProductParties
{
    public long Id { get; set; }
    public FinishedProducts FinishedProducts { get; set; } = null!;
    public long IdFinishedProduct { get; set; }
}