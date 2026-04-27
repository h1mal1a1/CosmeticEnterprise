using CosmeticEnterpriseBack.DTO.FinishedProductImages;

namespace CosmeticEnterpriseBack.Interfaces;

public interface IFinishedProductImageService
{
    Task<IReadOnlyList<FinishedProductImageResponse>> UploadAsync(long finishedProductId, UploadFinishedProductImageRequest request, 
        CancellationToken cancellationToken = default);

    Task DeleteAsync(long finishedProductId, long imageId, CancellationToken cancellationToken = default);
}