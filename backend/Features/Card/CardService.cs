namespace Backend.Features.Card;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IImageClient imageClient) : ICardService
{
    public readonly ILogger<ICardService> _logger = logger;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IImageClient _imageClient = imageClient;

    public async Task<int> CreateCard(int playerId, CreateCardDto cardDto)
    {
        try
        {
            string name = cardDto.Name!;
            IFormFile? file = cardDto.Image;

            if (file == null || file.Length == 0)
                throw new ArgumentNullException($"No image present!");

            string imageUrl = await _imageClient.Upload(file!);
            CardEntity card = new CardEntity(playerId);
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
            CardEntity card = await _cardRepository.GetCardById(cardId);

            await _cardRepository.DeleteCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while deleting Card with id {cardId}. (CardService)");
            throw;
        }
    }

    public async Task<IEnumerable<CardEntity>> GetAllCards(int playerId)
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