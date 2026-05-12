namespace CosmeticEnterpriseBack.Application.DTOs.FinishedProductImages;

public class UploadFinishedProductImageRequest
{
    public IReadOnlyCollection<UploadFinishedProductImageFileRequest> Files { get; init; } = [];
}