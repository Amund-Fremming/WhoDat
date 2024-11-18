using Backend.Features.BoardCard;
using Backend.Features.Database;

namespace Backend.Features.Board;

public class BoardRepository(AppDbContext context, ILogger<IBoardRepository> logger) : IBoardRepository
{
    public readonly AppDbContext _context = context;
    private readonly ILogger<IBoardRepository> _logger = logger;

    public async Task<BoardEntity> GetBoardById(int boardId)
    {
        return await _context.Board
            .Include(b => b.BoardCards)
            .FirstOrDefaultAsync(b => b.BoardID == boardId) ?? throw new KeyNotFoundException($"Board with id {boardId}, does not exist!");
    }

    public async Task<int> CreateBoard(BoardEntity board)
    {
        try
        {
            await _context.Board.AddAsync(board);
            await _context.SaveChangesAsync();

            return board.BoardID;
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error creating board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task DeleteBoard(BoardEntity board)
    {
        try
        {
            _context.Board.Remove(board);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error deleting board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task ChooseBoardCard(BoardEntity board, BoardCardEntity boardCard)
    {
        try
        {
            board.ChosenCard = boardCard;
            board.ChosenCardID = boardCard.BoardCardID;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error setting ChosenCard in board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task UpdateBoardCardsLeft(BoardEntity board, int playersLeft)
    {
        try
        {
            board.PlayersLeft = playersLeft;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error updating players left in board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }
}