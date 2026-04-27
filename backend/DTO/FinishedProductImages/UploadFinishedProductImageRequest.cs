namespace CosmeticEnterpriseBack.DTO.FinishedProductImages;

public class UploadFinishedProductImageRequest
{
    public List<IFormFile> Files { get; set; } = [];
}