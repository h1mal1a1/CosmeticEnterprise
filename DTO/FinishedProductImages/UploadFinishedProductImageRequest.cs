namespace CosmeticEnterpriseBack.DTO.FinishedProductImages;

public class UploadFinishedProductImageRequest
{
    public IFormFile File { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsMain { get; set; }
}