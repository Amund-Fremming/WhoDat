using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public class CardService(ILogger<ICardService> logger, ICardRepository cardRepository, IImageClient imageClient) : ICardService
{
    private readonly ILogger<ICardService> _logger = logger;
    private readonly ICardRepository _cardRepository = cardRepository;
    private readonly IImageClient _imageClient = imageClient;

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
                return result.Error;

            var imageUrl = result.Data;
            CardEntity card = new(playerId)
            {
                Name = name,
                Url = imageUrl
            };

            var cardResult = await _cardRepository.Create(card);
            if (cardResult.IsError)
                return cardResult.Error;

            return Result.Ok();
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
            var result = await _cardRepository.GetById(cardId);
            if (result.IsError)
                return result.Error;

            var card = result.Data;
            if (card.PlayerID != playerId)
                return new Error(new UnauthorizedAccessException("Not this players card."), "Cannot delete other players cards.");

            return await _cardRepository.Delete(card);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteCard)");
            return new Error(e, "Failed to delete card.");
        }
    }
}