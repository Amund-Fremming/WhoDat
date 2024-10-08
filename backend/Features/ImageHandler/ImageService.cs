using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace Image;

public class ImageService : IImageService
{
    public readonly IAmazonS3 _s3Client;
    public readonly IConfiguration _configuration;
    public readonly string BucketName = "whodat-image-container";
    public readonly string PublicUrlBase;

    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;

        PublicUrlBase = configuration["CloudflareR2:PublicUrlBase"]!;

        string accessKey = _configuration["CloudflareR2:AccessKey"]!;
        string secretKey = _configuration["CloudflareR2:SecretKey"]!;
        string accountId = _configuration["CloudflareR2:AccountId"]!;

        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
        });
    }

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
