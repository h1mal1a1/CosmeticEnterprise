using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CosmeticEnterpriseBack.Services.Storage;

public class MinioObjectStorageService : IObjectStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ObjectStorageSettings _settings;

    public MinioObjectStorageService(IMinioClient minioClient, IOptions<ObjectStorageSettings> settings)
    {
        _minioClient = minioClient;
        _settings = settings.Value;
    }

    public async Task<string> UploadAsync(Stream stream, string objectKey, string contentType, long size,
        CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectKey)
            .WithStreamData(stream)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        return objectKey;
    }

    public async Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectKey);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }

    public string GetFileUrl(string objectKey) => $"{_settings.PublicBaseUrl}/{_settings.BucketName}/{objectKey}";

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_settings.BucketName);

        var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        if (exists) return;

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(_settings.BucketName);
        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }
}