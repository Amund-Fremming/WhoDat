using GalleryEntity;

namespace CardEntity;

public class CardService(ILogger<CardService> logger, CardRepository cardRepository, GalleryRepository galleryRepository) : ICardService
{
    public readonly ILogger<CardService> _logger = logger;
    public readonly CardRepository _cardRepository = cardRepository;
    public readonly GalleryRepository _galleryRepository = galleryRepository;

    public async Task<int> CreateCard(int playerId, Card card)
    {
        try
        {
            Gallery gallery = await _galleryRepository.GetGalleryById(card.GalleryID);
            PlayerHasPermission(playerId, gallery);

            return await _cardRepository.CreateCard(card);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while creating Card with id {card.CardID}. (CardService)");
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
    public async Task UpdateCard(int playerId, Card newCard)
    {
        try
        {
            Card oldCard = await _cardRepository.GetCardById(newCard.CardID);
            Gallery gallery = await _galleryRepository.GetGalleryById(newCard.GalleryID);
            PlayerHasPermission(playerId, gallery);

            await _cardRepository.UpdateCard(oldCard, newCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while updating Card with id {newCard.CardID}. (CardService)");
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

    public void PlayerHasPermission(int playerId, Gallery gallery)
    {
        if (playerId != gallery.PlayerID)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}
