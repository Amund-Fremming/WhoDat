using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.BoardCard;

public class BoardCardRepository(AppDbContext context, ILogger<IBoardCardRepository> logger) : IBoardCardRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IBoardCardRepository> _logger = logger;

    public async Task<Result<BoardCardEntity>> GetBoardCardById(int boardCardId)
    {
        try
        {
            var boardCard = await _context.BoardCard.FindAsync(boardCardId);
            if (boardCard == null)
                return new Error(new KeyNotFoundException("Boardcard id does not exist."), "Board card does not exist.");

            return boardCard;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardCardById)");
            return new Error(e, "Failed to get board card.");
        }
    }

    public async Task<Result> CreateBoardCards(IEnumerable<BoardCardEntity> boardCards)
    {
        try
        {
            await _context.BoardCard.AddRangeAsync(boardCards);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateBoardCards)");
            return new Error(e, "Failed to create board cards.");
        }
    }

    public async Task<Result> UpdateBoardCardsActivity(IDictionary<int, bool> updateMap, IEnumerable<BoardCardEntity> boardCards)
    {
        try
        {
            foreach (var boardCard in boardCards)
            {
                if (updateMap.TryGetValue(boardCard.BoardCardID, out bool update))
                {
                    boardCard.Active = update;
                }
            }

            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsActivity)");
            return new Error(e, "Failed to update active board cards.");
        }
    }

    public async Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int boardId)
    {
        try
        {
            var cards = await _context.BoardCard
                .Where(b => b.BoardID == boardId)
                .Include(b => b.Card)
                .ToListAsync();

            return cards;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardCardsFromBoard)");
            return new Error(e, "Failed to get boardcards for board.");
        }
    }
}