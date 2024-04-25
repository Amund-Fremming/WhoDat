using Data;
using Microsoft.EntityFrameworkCore;

namespace BoardCardEntity;

public class BoardCardRepository(AppDbContext context, ILogger<BoardCardRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<BoardCardRepository> _logger = logger;

    public async Task<BoardCard> GetBoardCardById(int boardCardId)
    {
        return await _context.BoardCard
            .FindAsync(boardCardId) ?? throw new KeyNotFoundException($"BoardCard with id {boardCardId}, does not exist!");
    }

    public async Task<bool> CreateBoardCards(IEnumerable<BoardCard> boardCards)
    {
        try
        {
            await _context.BoardCard.AddRangeAsync(boardCards);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating BoardCards for Board. (BoardCardRepository)");
            return false;
        }
    }

    public async Task<bool> UpdateBoardCardsActivity(IDictionary<int, bool> updateMap, IEnumerable<BoardCard> boardCards)
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
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating BoardCards. (BoardCardRepository)");
            return false;
        }
    }

    public async Task<IList<BoardCard>> GetBoardCardsFromBoard(int boardId)
    {
        try
        {
            return await _context.BoardCard
                .Where(b => b.BoardID == boardId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error fetching BoardCards from Board with id {boardId}. (BoardCardRepository)");
            return null!;
        }
    }
}
