namespace CosmeticEnterpriseBack.Api.DTOs.FinishedProductImages;

public class UploadFinishedProductImageFormRequest
{
    public List<IFormFile> Files { get; set; } = [];
}