namespace CardEntity;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IImageService imageService) : ICardService
{
    public readonly ILogger<ICardService> _logger = logger;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IImageService _imageService = imageService;

    public async Task<int> CreateCard(int playerId, CardInputDto cardDto)
    {
        try
        {
            string name = cardDto.Name!;
            IFormFile? file = cardDto.Image;

            if (file == null || file.Length == 0)
                throw new ArgumentNullException($"No image present!");

            string imageUrl = await _imageService.Upload(file!);
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
}
