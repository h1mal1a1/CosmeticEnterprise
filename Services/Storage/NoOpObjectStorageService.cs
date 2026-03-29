using CosmeticEnterpriseBack.Interfaces;

namespace CosmeticEnterpriseBack.Services.Storage;

public class NoOpObjectStorageService : IObjectStorageService
{
    public Task<string> UploadAsync(
        Stream stream,
        string objectKey,
        string contentType,
        long size,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(string.Empty);
    }

    public Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public string GetFileUrl(string objectKey)
    {
        return string.Empty;
    }
}