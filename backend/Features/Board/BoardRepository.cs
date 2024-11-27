using Backend.Features.BoardCard;
using Backend.Features.Database;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public class BoardRepository(AppDbContext context, ILogger<BoardRepository> logger)
    : RepositoryBase<BoardEntity, BoardRepository>(logger, context), IBoardRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<BoardRepository> _logger = logger;

    public async Task<Result<BoardEntity>> GetBoardById(int boardId)
    {
        try
        {
            var board = await _context.Board.FindAsync(boardId);

            if (board == null)
                return new Error(new KeyNotFoundException("Board with id does not exist"), "The board does not exist.");

            return board;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardById)");
            return new Error(e, "System error.");
        }
    }

    public async Task<Result<int>> CreateBoard(BoardEntity board)
    {
        try
        {
            await _context.Board.AddAsync(board);
            var id = await _context.SaveChangesAsync();

            return id;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateBoard)");
            return new Error(e, "Creation of board failed.");
        }
    }

    public async Task<Result> DeleteBoard(BoardEntity board)
    {
        try
        {
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteBoard)");
            return new Error(e, "Failed to delete board.");
        }
    }

    public async Task<Result> ChooseBoardCard(BoardEntity board, BoardCardEntity boardCard)
    {
        try
        {
            board.ChosenCard = boardCard;
            board.ChosenCardID = boardCard.ID;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(ChooseBoardCard)");
            return new Error(e, "Failed to choose board card.");
        }
    }

    public async Task<Result> UpdateBoardCardsLeft(BoardEntity board, int playersLeft)
    {
        try
        {
            board.PlayersLeft = playersLeft;

            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsLeft)");
            return new Error(e, "Failed to update board cards left.");
        }
    }
}