namespace CardEntity;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IGalleryRepository galleryRepository, IHttpClientFactory httpClientFactory) : ICardService
{
    public readonly ILogger<ICardService> _logger = logger;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IGalleryRepository _galleryRepository = galleryRepository;
    public readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<int> CreateCard(int playerId, CardInputDto cardDto)
    {
        try
        {
            Card card = cardDto.Card!;
            IFormFile? file = cardDto.Image;

            Gallery gallery = await _galleryRepository.GetGalleryById(card.GalleryID);
            PlayerHasPermission(playerId, gallery);

            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException($"No image present!");
            }

            string imageUrl = await UploadImageToCloudflare(file!);
            card.Url = imageUrl;

            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while creating Card with id {cardDto.Card!.CardID}. (CardService)");
            throw;
        }
    }

    public async Task DeleteCard(int playerId, int cardId)
    {
        try
        {
            Card card = await _cardRepository.GetCardById(cardId);
            Gallery gallery = await _galleryRepository.GetGalleryById(card.GalleryID);
            PlayerHasPermission(playerId, gallery);

            await _cardRepository.DeleteCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while deleting Card with id {cardId}. (CardService)");
            throw;
        }
    }

    // RM - Maybe make players pay for more cards?
    public async Task UpdateCard(int playerId, CardInputDto newCardDto)
    {
        try
        {
            Card newCard = newCardDto.Card!;
            IFormFile? file = newCardDto.Image;

            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException($"No image present!");
            }

            string imageUrl = await UploadImageToCloudflare(file);
            newCard.Url = imageUrl;

            Card oldCard = await _cardRepository.GetCardById(newCard.CardID);
            Gallery gallery = await _galleryRepository.GetGalleryById(newCard.GalleryID);
            PlayerHasPermission(playerId, gallery);

            await _cardRepository.UpdateCard(oldCard, newCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while updating Card with id {newCardDto.Card!.CardID}. (CardService)");
            throw;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards(int playerId, int galleryId)
    {
        try
        {
            Gallery gallery = await _galleryRepository.GetGalleryById(galleryId);
            PlayerHasPermission(playerId, gallery);

            return await _cardRepository.GetAllCards(galleryId);
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

    public void PlayerHasPermission(int playerId, Gallery gallery)
    {
        if (playerId != gallery.PlayerID)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}
