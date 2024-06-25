namespace CardEntity;

public class CardRepository(AppDbContext context, ILogger<ICardRepository> logger) : ICardRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<ICardRepository> _logger = logger;

    public async Task<Card> GetCardById(int cardId)
    {
        return await _context.Card
            .FindAsync(cardId) ?? throw new KeyNotFoundException($"Card with id {cardId}, does not exist!");
    }

    public async Task<int> CreateCard(Card card)
    {
        try
        {
            await _context.Card.AddAsync(card);
            await _context.SaveChangesAsync();

            return card.CardID;
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error creating Card with id {card.CardID} .(CardRepository)");
            throw;
        }
    }

    public async Task DeleteCard(Card card)
    {
        try
        {
            _context.Remove(card);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error deleting Card with id {card.CardID} .(CardRepository)");
            throw;
        }
    }

    // RM - Maybe make players pay for more cards?
    public async Task UpdateCard(Card oldCard, Card newCard)
    {
        try
        {
            oldCard.GalleryID = newCard.GalleryID;
            oldCard.Gallery = newCard.Gallery;
            oldCard.Name = newCard.Name;
            oldCard.Url = newCard.Url;

            _context.Card.Update(oldCard);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error updating Card with id {oldCard.CardID} .(CardRepository)");
            throw;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards(int galleryId)
    {
        return await _context.Card
            .Where(c => c.GalleryID == galleryId)
            .ToListAsync();
    }
}
