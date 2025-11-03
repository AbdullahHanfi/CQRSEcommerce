
namespace Application.Abstractions.Services;

/// <summary>
/// use to intreacte with files
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// upload file using passed filename
    /// </summary>
    /// <param name="fileStream"> stream for file</param>
    /// <param name="fileName">file store name</param>
    /// <param name="contentType">file content type</param>
    /// <returns>true for success or false with reason of failure</returns>
    Task<Result> UploadStreamAsync(Stream fileStream, string fileName, string contentType);
    /// <summary>
    /// get temporary link for file
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="expirationTime">when will link disable</param>
    /// <returns>URL for success or false with reason of failure</returns>
    Task<Result<string>> GetFileURLAsync(string fileName,DateTime expirationTime);
    Task<Result> DeleteFileAsync(string fileName);
}
