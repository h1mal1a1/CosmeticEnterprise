using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.FinishedProductImages;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.FinishedProductImages;

public class FinishedProductImageService : IFinishedProductImageService
{
    private static readonly HashSet<string> AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];
    private readonly AppDbContext _dbContext;
    private readonly IObjectStorageService _objectStorageService;

    public FinishedProductImageService(AppDbContext dbContext, IObjectStorageService objectStorageService)
    {
        _dbContext = dbContext;
        _objectStorageService = objectStorageService;
    }

    public async Task<FinishedProductImageResponse> UploadAsync(long finishedProductId,
        UploadFinishedProductImageRequest request, CancellationToken cancellationToken = default)
    {
        var productExists =
            await _dbContext.FinishedProducts.AnyAsync(x => x.Id == finishedProductId, cancellationToken);
        if (!productExists)
            throw new InvalidOperationException($"Finished product with id {finishedProductId} not found");
        if (request.File is null || request.File.Length == 0)
            throw new InvalidOperationException("File is required");
        if(!AllowedContentTypes.Contains(request.File.ContentType))
            throw new InvalidOperationException("Unsupported file type");
        const long maxFileSize = 5 * 1024 * 1024;
        if(request.File.Length > maxFileSize)
            throw new InvalidOperationException("File size must be less then or equal to 5MB");
        var extension = GetExtension(request.File.ContentType);
        var objectKey = $"products/{finishedProductId}/{Guid.NewGuid():N}{extension}";
        await using var stream = request.File.OpenReadStream();
        await _objectStorageService.UploadAsync(stream, objectKey, request.File.ContentType, request.File.Length,
            cancellationToken);
        if (request.IsMain)
        {
            var currentMainImages = await _dbContext.FinishedProductImages
                .Where(x => x.IdFinishedProduct == finishedProductId && x.IsMain)
                .ToListAsync(cancellationToken);
            foreach (var image in currentMainImages)
                image.IsMain = false;
        }

        var isFirstImage =
            !await _dbContext.FinishedProductImages
                .AnyAsync(x => x.IdFinishedProduct == finishedProductId,cancellationToken);
        var entity = new Entities.FinishedProductImages
        {
            IdFinishedProduct = finishedProductId,
            ObjectKey = objectKey,
            SortOrder = request.SortOrder,
            IsMain = request.IsMain || isFirstImage
        };

        _dbContext.FinishedProductImages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new FinishedProductImageResponse
        {
            Id = entity.Id,
            FileUrl = _objectStorageService.GetFileUrl(entity.ObjectKey),
            SortOrder = entity.SortOrder,
            IsMain = entity.IsMain
        };
    }

    public async Task DeleteAsync(long finishedProductId, long imageId, CancellationToken cancellationToken = default)
    {
        var image = await _dbContext.FinishedProductImages
            .FirstOrDefaultAsync(
                x => x.Id == imageId && x.IdFinishedProduct == finishedProductId,
                cancellationToken);

        if (image is null)
            throw new InvalidOperationException($"Image with id {imageId} not found");

        _dbContext.FinishedProductImages.Remove(image);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _objectStorageService.DeleteAsync(image.ObjectKey, cancellationToken);
    }

    private static string GetExtension(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new InvalidOperationException("Unsupported file type")
        };
    }
}