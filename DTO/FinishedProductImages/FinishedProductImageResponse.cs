namespace CosmeticEnterpriseBack.DTO.FinishedProductImages;

public class FinishedProductImageResponse
{
    public long Id { get; set; }
    public string FileUrl { get; set; } = null!;
    public int SortOrder { get; set; } 
    public bool IsMain { get; set; } 
}