using Backend.Features.BoardCard;
using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public class BoardRepository(AppDbContext context, ILogger<IBoardRepository> logger) : IBoardRepository
{
    public readonly AppDbContext _context = context;
    private readonly ILogger<IBoardRepository> _logger = logger;

    public async Task<Result<BoardEntity>> GetBoardById(int boardId)
    {
        try
        {
            var board = await _context.Board
                .Include(b => b.BoardCards)
                .FirstOrDefaultAsync(b => b.BoardID == boardId);

            if (board == null)
                return (new KeyNotFoundException("Board with id does not exist"), "The board does not exist.");

            return board;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardById)");
            return (e, "System error. Please try again later.");
        }
    }

    public async Task<Result<BoardEntity>> CreateBoard(BoardEntity board)
    {
        try
        {
            await _context.Board.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateBoard)");
            return (e, "Creation of board failed. Please try again later.");
        }
    }

    public async Task<Result> DeleteBoard(BoardEntity board)
    {
        try
        {
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteBoard)");
            return (e, "Failed to delete board. Please try again later.");
        }
    }

    public async Task<Result> ChooseBoardCard(BoardEntity board, BoardCardEntity boardCard)
    {
        try
        {
            board.ChosenCard = boardCard;
            board.ChosenCardID = boardCard.BoardCardID;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(ChooseBoardCard)");
            return (e, "Failed to choose board card. Please try again later.");
        }
    }

    public async Task<Result> UpdateBoardCardsLeft(BoardEntity board, int playersLeft)
    {
        try
        {
            board.PlayersLeft = playersLeft;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsLeft)");
            return (e, "Failed to update board cards left. Please try again later");
        }
    }
}