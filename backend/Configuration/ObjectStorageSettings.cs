namespace CosmeticEnterpriseBack.Configuration;

public class ObjectStorageSettings
{
  public string Endpoint { get; set; } = string.Empty;
  public string PublicBaseUrl { get; set; } = null!;
  public string AccessKey { get; set; } = string.Empty;
  public string SecretKey { get; set; } = string.Empty;
  public string BucketName { get; set; } = string.Empty;
  public bool UseSsl { get; set; }
}