namespace CosmeticEnterpriseBack.Entities;

public class FinishedProductImages
{
    public long Id { get; set; }
    public long IdFinishedProduct { get; set; }
    public string ObjectKey { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsMain { get; set; }
    public FinishedProducts FinishedProduct { get; set; } = null!;
}