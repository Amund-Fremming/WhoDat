namespace Image;

public interface IImageService
{
    /// <summary>
    /// Uploads a image to CloudFlare Bucket and returns the URL for the image.
    /// </summary>
    Task<string> Upload(IFormFile form);
}
