namespace CardEntity;

public class CardService(ILogger<CardService> logger, CardRepository cardRepository) : ICardService
{
    public readonly CardRepository _cardRepository = cardRepository;
    public readonly ILogger<CardService> _logger = logger;

    public async Task<int> CreateCard(Card card)
    {
        try
        {
            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Card with id {card.CardID}. (CardService)");
            throw;
        }
    }

    public async Task<bool> DeleteCard(int cardId)
    {
        try
        {
            Card card = await _cardRepository.GetCardById(cardId);

            return await _cardRepository.DeleteCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Card with id {cardId}. (CardService)");
            throw;
        }
    }

    public async Task<bool> UpdateCard(Card newCard)
    {
        try
        {
            Card oldCard = await _cardRepository.GetCardById(newCard.CardID);

            return await _cardRepository.UpdateCard(oldCard, newCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating Card with id {newCard.CardID}. (CardService)");
            throw;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards()
    {
        try
        {
            return await _cardRepository.GetAllCards();
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while getting all cards. (CardService)");
            throw;
        }
    }
}
