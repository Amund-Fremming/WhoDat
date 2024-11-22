using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public class CardRepository(AppDbContext context, ILogger<ICardRepository> logger) : ICardRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<ICardRepository> _logger = logger;

    public async Task<Result<CardEntity>> GetCardById(int cardId)
    {
        var card = await _context.Card.FindAsync(cardId);
        if (card == null)
            return new Error(new KeyNotFoundException($"Card with id {cardId}, does not exist!"), "Card does not exist.");

        return card;
    }

    public async Task<Result<int>> CreateCard(CardEntity card)
    {
        try
        {
            await _context.Card.AddAsync(card);
            await _context.SaveChangesAsync();
            return card.ID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateCard)");
            return new Error(e, "Failed to create card.");
        }
    }

    public async Task<Result> DeleteCard(CardEntity card)
    {
        try
        {
            _context.Remove(card);
            await _context.SaveChangesAsync();
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
            return await _context.Card
                .Where(c => c.PlayerID == playerId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetAllCards)");
            return new Error(e, "Failed to get all cards");
        }
    }
}