
namespace Application.Abstractions.Services
{
    public interface IImageService
    {
        Task<Result<string>> UploadImageAsync(Stream stream, string contentType);
        Task<Result<string>> GetImageURLAsync(string imageName);
        Task<Result> DeleteAsync(string imageName);
    }
}
