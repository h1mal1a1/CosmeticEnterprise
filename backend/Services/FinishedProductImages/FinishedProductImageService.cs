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

    public FinishedProductImageService(
        AppDbContext dbContext,
        IObjectStorageService objectStorageService)
    {
        _dbContext = dbContext;
        _objectStorageService = objectStorageService;
    }

    public async Task<IReadOnlyList<FinishedProductImageResponse>> UploadAsync(
        long finishedProductId,
        UploadFinishedProductImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var productExists = await _dbContext.FinishedProducts
            .AnyAsync(x => x.Id == finishedProductId, cancellationToken);

        if (!productExists)
            throw new InvalidOperationException($"Finished product with id {finishedProductId} not found");

        if (request.Files is null || request.Files.Count == 0)
            throw new InvalidOperationException("Files are required");

        var responses = new List<FinishedProductImageResponse>();

        var isFirstImage = !await _dbContext.FinishedProductImages
            .AnyAsync(x => x.IdFinishedProduct == finishedProductId, cancellationToken);

        var currentMaxOrder = isFirstImage
            ? 0
            : await _dbContext.FinishedProductImages
                .Where(x => x.IdFinishedProduct == finishedProductId)
                .Select(x => x.SortOrder)
                .MaxAsync(cancellationToken);

        foreach (var file in request.Files)
        {
            if (file is null || file.Length == 0)
                continue;

            if (!AllowedContentTypes.Contains(file.ContentType))
                throw new InvalidOperationException("Unsupported file type");

            const long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
                throw new InvalidOperationException("File size must be <= 5MB");

            var extension = GetExtension(file.ContentType);
            var objectKey = $"products/{finishedProductId}/{Guid.NewGuid():N}{extension}";

            await using var stream = file.OpenReadStream();

            await _objectStorageService.UploadAsync(
                stream,
                objectKey,
                file.ContentType,
                file.Length,
                cancellationToken);

            currentMaxOrder++;

            var entity = new Entities.FinishedProductImages
            {
                IdFinishedProduct = finishedProductId,
                ObjectKey = objectKey,
                SortOrder = currentMaxOrder,
                IsMain = isFirstImage && responses.Count == 0
            };

            _dbContext.FinishedProductImages.Add(entity);

            responses.Add(new FinishedProductImageResponse
            {
                Id = entity.Id,
                FileUrl = _objectStorageService.GetFileUrl(objectKey),
                SortOrder = entity.SortOrder,
                IsMain = entity.IsMain
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return responses;
    }

    public async Task<IReadOnlyList<FinishedProductImageResponse>> SetMainAsync(
        long finishedProductId,
        long imageId,
        CancellationToken cancellationToken = default)
    {
        var images = await _dbContext.FinishedProductImages
            .Where(x => x.IdFinishedProduct == finishedProductId)
            .ToListAsync(cancellationToken);

        if (images.Count == 0)
            throw new InvalidOperationException("No images found");

        var target = images.FirstOrDefault(x => x.Id == imageId);

        if (target is null)
            throw new InvalidOperationException("Image not found");

        foreach (var img in images)
            img.IsMain = false;

        target.IsMain = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return images.Select(x => new FinishedProductImageResponse
        {
            Id = x.Id,
            FileUrl = _objectStorageService.GetFileUrl(x.ObjectKey),
            SortOrder = x.SortOrder,
            IsMain = x.IsMain
        }).ToList();
    }

    public async Task DeleteAsync(
        long finishedProductId,
        long imageId,
        CancellationToken cancellationToken = default)
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