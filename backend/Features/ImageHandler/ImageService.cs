namespace Image;

public class ImageService(IHttpClientFactory httpClientFactory) : IImageService
{
    public readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<string> Upload(IFormFile file)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        // string containerEndpoint = "https://whodat-image-worker.amund-fremming.workers.dev/";
        string containerEndpoint = "https://0b57699fd3dc67e5eadbdce80ca506a6.r2.cloudflarestorage.com/whodat-image-container";

        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var response = await client.PutAsync(containerEndpoint, content);
            Console.WriteLine("Response: " + response);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ToString()!;
            }
            else
            {
                throw new Exception("Failed to upload image");
            }
        }
    }
}
