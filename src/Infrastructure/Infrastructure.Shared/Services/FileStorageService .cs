using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions.Services;
using Domain.Shared.Results;
using Infrastructure.Shared.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Shared.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName;
    private readonly ILogger<FileStorageService> _logger;


    public FileStorageService(IOptions<DigitalOceanSpaceSettings> cloudSettings, ILogger<FileStorageService> logger)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = cloudSettings.Value.ServiceUrl,
            ForcePathStyle = true
        };
        _bucketName = cloudSettings.Value.BucketName;
        _client = new AmazonS3Client(cloudSettings.Value.AccessKey, cloudSettings.Value.SecretKey, config);
        _logger = logger;
    }
    public async Task<Result> UploadStreamAsync(Stream fileStream, string fileName, string contentType)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.Private
            };

            var response = await _client.PutObjectAsync(putRequest);
            return
                response.HttpStatusCode == System.Net.HttpStatusCode.OK
                ? Result.Success()
                : Result.Failure(new Error("Didn't upload"));
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error uploading stream: {Message}", ex.Message);
            return Result.Failure(new(ex.Message));
        }
    }
    public async Task<Result<string>> GetFileURLAsync(string fileName, DateTime expirationTime)
    {
        try
        {
            var putRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = expirationTime
            };

            var response = await _client.GetPreSignedURLAsync(putRequest);
            return response;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error getting URL: {Message}", ex.Message);
            return Result.Failure<string>(new(ex.Message));
        }
    }

    public async Task<Result> DeleteFileAsync(string fileName)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _client.DeleteObjectAsync(deleteRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent
                ? Result.Success()
                : Result.Failure(new Error("Didn't delete"));

        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error while deleting file: {Message}", ex.Message);
            return Result.Failure(new Error(ex.Message));
        }
    }

}
