using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Services.Storage;
using Minio;

namespace CosmeticEnterpriseBack.Extensions;

public static class StorageExtensions 
{
    public static void AddMinio(this WebApplicationBuilder builder)
    {
        var objectStorageSection = builder.Configuration.GetSection("ObjectStorage");
        if (!objectStorageSection.Exists())
        {
            builder.Services.AddScoped<IObjectStorageService, NoOpObjectStorageService>();
            return;
        }
        
        var objectStorageSettings = objectStorageSection.Get<ObjectStorageSettings>()
                                    ?? throw new Exception("ObjectStorage settings not found");
        if (string.IsNullOrWhiteSpace(objectStorageSettings.Endpoint) ||
            string.IsNullOrWhiteSpace(objectStorageSettings.AccessKey) ||
            string.IsNullOrWhiteSpace(objectStorageSettings.SecretKey) ||
            string.IsNullOrWhiteSpace(objectStorageSettings.BucketName))
        {
            builder.Services.AddScoped<IObjectStorageService, NoOpObjectStorageService>();
            return;
        }


        builder.Services.Configure<ObjectStorageSettings>(objectStorageSection);
        builder.Services.AddSingleton<IMinioClient>(_ =>
            new MinioClient()
                .WithEndpoint(objectStorageSettings.Endpoint)
                .WithCredentials(objectStorageSettings.AccessKey, objectStorageSettings.SecretKey)
                .WithSSL(objectStorageSettings.UseSsl)
                .Build());

        builder.Services.AddScoped<IObjectStorageService, MinioObjectStorageService>();
    }
}