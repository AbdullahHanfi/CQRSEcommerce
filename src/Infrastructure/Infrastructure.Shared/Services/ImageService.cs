using Application.Abstractions.Services;
using Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Shared.Services;


public class ImageService(IFileStorageService fileStorageService) : IImageService
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<Result<string>> GetImageURLAsync(string imageName)
    {
        return await _fileStorageService.GetFileURLAsync(imageName, DateTime.UtcNow.AddMinutes(5));
    }
    public async Task<Result<string>> UploadImageAsync(Stream stream, string contentType)
    {
        if (stream == null || stream.Length == 0)
        {
            return Result.Failure<string>(new("File is empty or null."));
        }

        var fileExtention = IsValidImageFile(stream);

        if (fileExtention.IsFailure)
        {
            return Result.Failure<string>(new($"Not image"));
        }

        string fileName = Guid.NewGuid().ToString() + fileExtention.Value;

        var imageState = await _fileStorageService.UploadStreamAsync(stream, fileName, contentType);

        return imageState.IsSuccess
            ? fileName
            : Result.Failure<string>(imageState.Error);
    }

    public async Task<Result> DeleteAsync(string imageName)
    {
        if (string.IsNullOrEmpty(imageName))
        {
            return Result.Failure(new("Image name is null or empty."));
        }

        return await _fileStorageService.DeleteFileAsync(imageName);
    }

    private Result<string> IsValidImageFile(Stream stream)
    {

        var signatures = new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                    }
                },
                { ".jpg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                    }
                },
                { ".png", new List<byte[]>
                    {
                        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                    }
                },
                { ".webp", new List<byte[]>
                    {
                        new byte[] { 0x52, 0x49, 0x46, 0x46 }
                    }
                }
            };

        var headerBytes = new byte[8];
        stream.Read(headerBytes, 0, headerBytes.Length);
        stream.Position = 0;
        foreach (var signature in signatures)
        {
            foreach (var Bytes in signature.Value)
            {
                if (headerBytes.Take(Bytes.Length).SequenceEqual(Bytes))
                {
                    return signature.Key;
                }
            }
        }

        return Result.Failure<string>(new("UnValid Image"));
    }
}
