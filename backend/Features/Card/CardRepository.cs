using Backend.Features.Database;

namespace Backend.Features.Card;

public class CardRepository(AppDbContext context, ILogger<ICardRepository> logger) : ICardRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<ICardRepository> _logger = logger;

    public async Task<CardEntity> GetCardById(int cardId)
    {
        return await _context.Card
            .FindAsync(cardId) ?? throw new KeyNotFoundException($"Card with id {cardId}, does not exist!");
    }

    public async Task<int> CreateCard(CardEntity card)
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

    public async Task DeleteCard(CardEntity card)
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

    public async Task<IEnumerable<CardEntity>> GetAllCards(int playerId)
    {
        return await _context.Card
            .Where(c => c.PlayerID == playerId)
            .ToListAsync();
    }
}