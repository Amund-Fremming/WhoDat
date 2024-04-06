using Data;

namespace BoardCardEntity;

public class BoardCardRepository(AppDbContext context, ILogger<BoardCardRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<BoardCardRepository> _logger = logger;

    public async Task<BoardCard?> GetBoardCardById(int boardId)
    {
        return await _context.BoardCard.FindAsync(boardId);
    }

    public async Task<int> CreateBoardCard(BoardCard boardCard)
    {
        try
        {
            await _context.BoardCard.AddAsync(boardCard);
            return boardCard.BoardCardID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating BoardCard with id {boardCard.BoardCardID} .(BoardCardRepository)");
            return -1;
        }
    }

    public async Task<bool> DeleteBoardCard(BoardCard boardCard)
    {
        try
        {
            _context.BoardCard.Remove(boardCard);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting BoardCard with id {boardCard.BoardCardID} .(BoardCardRepository)");
            return false;
        }
    }

    public async Task<bool> UpdateActive(BoardCard boardCard, bool active)
    {
        try
        {
            boardCard.Active = active;
            _context.BoardCard.Update(boardCard);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating BoardCard with id {boardCard.BoardCardID} .(BoardCardRepository)");
            return false;
        }
    }
}
