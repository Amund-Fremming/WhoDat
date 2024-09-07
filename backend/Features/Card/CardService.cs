namespace CardEntity;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IHttpClientFactory httpClientFactory) : ICardService
{
    public readonly ILogger<ICardService> _logger = logger;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<int> CreateCard(int playerId, CardInputDto cardDto)
    {
        try
        {
            string name = cardDto.Name!;
            IFormFile? file = cardDto.Image;

            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException($"No image present!");
            }

            string imageUrl = await UploadImageToCloudflare(file!);
            Card card = new Card(playerId);
            card.Name = name;
            card.Url = imageUrl;

            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while creating Card with player id {playerId}. (CardService)");
            throw;
        }
    }

    public async Task DeleteCard(int playerId, int cardId)
    {
        try
        {
            Card card = await _cardRepository.GetCardById(cardId);

            await _cardRepository.DeleteCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while deleting Card with id {cardId}. (CardService)");
            throw;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards(int playerId)
    {
        try
        {
            return await _cardRepository.GetAllCards(playerId);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while getting all cards. (CardService)");
            throw;
        }
    }

    private async Task<string> UploadImageToCloudflare(IFormFile file)
    {

        var client = _httpClientFactory.CreateClient();
        string containerEndpoint = "https://whodat-image-worker.amund-fremming.workers.dev/";

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
