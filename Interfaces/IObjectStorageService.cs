namespace CosmeticEnterpriseBack.Interfaces;

public interface IObjectStorageService
{
    Task<string> UploadAsync(
        Stream stream,
        string objectKey,
        string contentType,
        long size,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken = default);

    string GetFileUrl(string objectKey);
}