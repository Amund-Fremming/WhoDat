namespace BoardEntity;
using BoardCardEntity;
using Data;

public class BoardService(ILogger<BoardService> logger, AppDbContext context, BoardRepository boardRepository, BoardCardRepository boardCardRepository) : IBoardService
{
    public readonly ILogger<BoardService> _logger = logger;
    public readonly AppDbContext _context = context;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly BoardCardRepository _boardCardRepository = boardCardRepository;

    public async Task<int> CreateBoard(int playerId, Board board)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                board.PlayerID = playerId;
                int boardId = await _boardRepository.CreateBoard(board);

                await transaction.CommitAsync();
                return boardId;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while creating Board with id {board.BoardID}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task DeleteBoard(int playerId, int boardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                await _boardRepository.DeleteBoard(board);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while deleting Board with id {boardId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task ChooseCard(int playerId, int boardId, int boardCardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);
                BoardCard boardCard = await _boardCardRepository.GetBoardCardById(boardCardId);

                await _boardRepository.ChooseCard(board, boardCard);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error chosing a card on Board with id {boardId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task UpdatePlayersLeft(int playerId, int boardId, int activePlayers)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            await _boardRepository.UpdatePlayersLeft(board, activePlayers);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error updating card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public async Task<Board> GetBoardWithBoardCards(int playerId, int boardId)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            return board;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error updating card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public void PlayerHasPermission(int playerId, Board board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }
}
