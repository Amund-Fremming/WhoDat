namespace BoardEntity;
using BoardCardEntity;
using Data;

public class BoardService(ILogger<BoardService> logger, AppDbContext context, BoardRepository boardRepository, BoardCardRepository boardCardRepository) : IBoardService
{
    public readonly ILogger<BoardService> _logger = logger;
    public readonly AppDbContext _context = context;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly BoardCardRepository _boardCardRepository = boardCardRepository;

    public async Task<int> CreateBoard(Board board)
    {
        return await _boardRepository.CreateBoard(board);
    }

    public async Task DeleteBoard(int boardId)
    {
        try
        {
            Board? board = await _boardRepository.GetBoardById(boardId);
            await _boardRepository.DeleteBoard(board);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public async Task ChooseCard(int boardId, int boardCardId)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            BoardCard boardCard = await _boardCardRepository.GetBoardCardById(boardCardId);

            await _boardRepository.ChooseCard(board, boardCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error chosing a card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public async Task UpdatePlayersLeft(int boardId, int activePlayers)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            await _boardRepository.UpdatePlayersLeft(board, activePlayers);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error updating card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }
}
