using Data;
using BoardCardEntity;

namespace BoardEntity;

public class BoardRepository(AppDbContext context, ILogger<BoardRepository> logger)
{
    public readonly AppDbContext _context = context;
    private readonly ILogger<BoardRepository> _logger = logger;

    public async Task<Board> GetBoardById(int boardId)
    {
        return await _context.Board
            .FindAsync(boardId) ?? throw new KeyNotFoundException($"Board with id {boardId}, does not exist!");
    }

    public async Task<int> CreateBoard(Board board)
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
            _logger.LogError(e, $"Error creating board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task DeleteBoard(Board board)
    {
        try
        {
            _context.Board.Remove(board);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error deleting board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task ChooseCard(Board board, BoardCard boardCard)
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
            _logger.LogError(e, $"Error setting ChosenCard in board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

    public async Task UpdatePlayersLeft(Board board, int playersLeft)
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
            _logger.LogError(e, $"Error updating players left in board with id {board.BoardID}. (BoardRepository)");
            throw;
        }
    }

}
