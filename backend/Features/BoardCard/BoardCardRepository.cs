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
                return (new KeyNotFoundException("Boardcard id does not exist."), "Board card does not exist.");

            return boardCard;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardCardById)");
            return (e, "Failed to get board card. Please try again later.");
        }
    }

    public async Task CreateBoardCards(IEnumerable<BoardCardEntity> boardCards)
    {
        try
        {
            await _context.BoardCard.AddRangeAsync(boardCards);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error creating BoardCards for Board. (BoardCardRepository)");
            throw;
        }
    }

    public async Task UpdateBoardCardsActivity(IDictionary<int, bool> updateMap, IEnumerable<BoardCardEntity> boardCards)
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
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error updating BoardCards. (BoardCardRepository)");
            throw;
        }
    }

    public async Task<IList<BoardCardEntity>> GetBoardCardsFromBoard(int boardId)
    {
        try
        {
            return await _context.BoardCard
                .Where(b => b.BoardID == boardId)
                .Include(b => b.Card)
                .ToListAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error fetching BoardCards from Board with id {boardId}. (BoardCardRepository)");
            throw;
        }
    }
}