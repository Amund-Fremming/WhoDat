using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface IImageClient
{
    Task<Result<string>> Upload(IFormFile form);
}

public class ImageClient : IImageClient
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImageClient> _logger;
    private readonly string BucketName;
    private readonly string PublicUrlBase;

    public ImageClient(IConfiguration configuration, ILogger<ImageClient> logger)
    {
        _configuration = configuration;
        _logger = logger;

        BucketName = _configuration["CloudflareR2:BucketName"]!;
        PublicUrlBase = _configuration["CloudflareR2:PublicUrlBase"]!;

        var accessKey = _configuration["CloudflareR2:AccessKey"]!;
        var secretKey = _configuration["CloudflareR2:SecretKey"]!;
        var accountId = _configuration["CloudflareR2:AccountId"]!;

        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
        });
    }

    public async Task<Result<string>> Upload(IFormFile file)
    {
        try
        {
            var imageKey = Guid.NewGuid().ToString();

            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = imageKey,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            _ = await _s3Client.PutObjectAsync(request);
            var imageUrl = $"{PublicUrlBase}/{imageKey}";
            return imageUrl;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(Upload)");
            return new Error(e, "Failed to upload image, try again.");
        }
    }
}