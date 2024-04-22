namespace CardEntity;

public class CardService(ILogger<CardService> logger, CardRepository cardRepository) : ICardService
{
    public readonly CardRepository _cardRepository = cardRepository;
    public readonly ILogger<CardService> _logger = logger;

    public async Task<int> CreateCard(int playerId, int galleryId, Card card)
    {
        try
        {
            // TODO - hasPermission
            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Card with id {card.CardID}. (CardService)");
            throw;
        }
    }

    public async Task DeleteCard(int playerId, int galleryId, int cardId)
    {
        try
        {
            Card card = await _cardRepository.GetCardById(cardId);
            // TODO - hasPermission

            await _cardRepository.DeleteCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Card with id {cardId}. (CardService)");
            throw;
        }
    }

    public async Task UpdateCard(int playerId, Card newCard)
    {
        try
        {
            Card oldCard = await _cardRepository.GetCardById(newCard.CardID);
            // TODO - hasPermission

            await _cardRepository.UpdateCard(oldCard, newCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating Card with id {newCard.CardID}. (CardService)");
            throw;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards(int playerId, int galleryId)
    {
        try
        {
            // TODO - hasPermission

            return await _cardRepository.GetAllCards(galleryId);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while getting all cards. (CardService)");
            throw;
        }
    }
}
