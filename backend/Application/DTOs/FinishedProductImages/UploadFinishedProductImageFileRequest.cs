namespace CosmeticEnterpriseBack.Application.DTOs.FinishedProductImages;

public class UploadFinishedProductImageFileRequest
{
    public Stream FileStream { get; init; } = Stream.Null;

    public string FileName { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long Length { get; init; }
}