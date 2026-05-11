namespace CosmeticEnterpriseBack.Application.DTOs.FinishedProductImages;
public class UploadFinishedProductImageRequest
{
    public List<IFormFile> Files { get; set; } = [];
}