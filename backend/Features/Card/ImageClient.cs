using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface IImageClient
{
    /// <summary>
    /// Uploads a image to CloudFlare Bucket and returns the URL for the image.
    /// </summary>
    Task<Result<string>> Upload(IFormFile form);
}

public class ImageClient : IImageClient
{
    public readonly IAmazonS3 _s3Client;
    public readonly IConfiguration _configuration;
    public readonly ILogger<ImageClient> _logger;
    public readonly string BucketName = "whodat-image-container";
    public readonly string PublicUrlBase;

    public ImageClient(IConfiguration configuration, ILogger<ImageClient> logger)
    {
        _configuration = configuration;
        _logger = logger;

        BucketName = _configuration["CloudflareR2:BucketName"]!;
        PublicUrlBase = _configuration["CloudflareR2:PublicUrlBase"]!;

        string accessKey = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("CLOUDFLARE_SECRET_KEY")!;
        string accountId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCOUNT_ID")!;

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
            string imageKey = Guid.NewGuid().ToString();

            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = imageKey,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            var response = await _s3Client.PutObjectAsync(request);

            string imageUrl = $"{PublicUrlBase}/{imageKey}";
            Console.WriteLine($"Image uploaded successfully. Access URL: {imageUrl}");

            return imageUrl;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(Upload)");
            return new Error(e, "Failed to upload image, try again.");
        }
    }

    /*
    public async Task<string> Upload(IFormFile file)
    {
        string imageKey = Guid.NewGuid().ToString();

        var request = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = imageKey,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType,
            DisablePayloadSigning = true
        };

        var response = await _s3Client.PutObjectAsync(request);

        string imageUrl = $"{PublicUrlBase}/{imageKey}";
        Console.WriteLine($"Image uploaded successfully. Access URL: {imageUrl}");

        return imageUrl;
    }
    */
}