using Backend.Features.Database;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public class CardRepository(ILogger<CardRepository> logger, AppDbContext context)
    : RepositoryBase<CardEntity, CardRepository>(logger, context), ICardRepository
{
    private readonly ILogger<CardRepository> _logger = logger;
    private readonly AppDbContext _context = context;

    public async Task<Result<IEnumerable<CardDto>>> GetAllCards(int playerId)
    {
        try
        {
            return await _context.Card
                .Where(c => c.PlayerID == playerId)
                .AsNoTracking()
                .Select(ce => new CardDto(ce.ID, ce.Name, ce.Url))
                .ToArrayAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetAllCards)");
            return new Error(e, "Failed to get all cards");
        }
    }
}