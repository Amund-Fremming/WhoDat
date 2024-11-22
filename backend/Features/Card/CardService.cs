using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IImageClient imageClient) : ICardService
{
    public readonly ILogger<ICardService> _logger = logger;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IImageClient _imageClient = imageClient;

    public async Task<Result> CreateCard(int playerId, CreateCardDto cardDto)
    {
        try
        {
            string name = cardDto.Name!;
            IFormFile? file = cardDto.Image;

            if (file == null || file.Length == 0)
                return new Error(new ArgumentNullException("No image present."), "No image present to be uploaded.");

            var result = await _imageClient.Upload(file!);
            if (result.IsError)
                return result;

            var imageUrl = result.Data;
            CardEntity card = new(playerId)
            {
                Name = name,
                Url = imageUrl
            };

            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateCard)");
            return new Error(e, "Failed to create card.");
        }
    }

    public async Task<Result> DeleteCard(int playerId, int cardId)
    {
        try
        {
            var result = await _cardRepository.GetCardById(cardId);
            if (result.IsError)
                return result;

            var card = result.Data;
            await _cardRepository.DeleteCard(card);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteCard)");
            return new Error(e, "Failed to delete card.");
        }
    }

    public async Task<Result<IEnumerable<CardEntity>>> GetAllCards(int playerId)
    {
        try
        {
            return await _cardRepository.GetAllCards(playerId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetAllCards)");
            return new Error(e, "Failed to get all cards.");
        }
    }
}