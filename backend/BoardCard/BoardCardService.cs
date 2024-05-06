using BoardEntity;
using CardEntity;
using Data;

namespace BoardCardEntity;

public class BoardCardService(AppDbContext context, ILogger<BoardCardService> logger,
        BoardCardRepository boardcardRepository, BoardRepository boardRepository, CardRepository cardRepository) : IBoardCardService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<BoardCardService> _logger = logger;
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly CardRepository _cardRepository = cardRepository;

    public async Task CreateBoardCards(int playerId, int boardId, IEnumerable<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                IEnumerable<BoardCard> newBoardCards = cardIds.Select(cardId => new BoardCard(boardId, cardId)).ToList();

                await _boardcardRepository.CreateBoardCards(newBoardCards);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while creating BoardCards for board with id {boardId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<int> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                IDictionary<int, bool> updateMap = boardCardUpdates.ToDictionary(update => update.BoardCardID, update => update.Active);
                IList<BoardCard> boardCards = await _boardcardRepository.GetBoardCardsFromBoard(boardId);
                int playersLeft = boardCards.Count(bc => bc.Active);

                await _boardcardRepository.UpdateBoardCardsActivity(updateMap, boardCards);
                await _boardRepository.UpdatePlayersLeft(board, playersLeft);

                await transaction.CommitAsync();
                return playersLeft;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<IEnumerable<BoardCard>> GetBoardCardsFromBoard(int playerId, int boardId)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            return await _boardcardRepository.GetBoardCardsFromBoard(boardId);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
            throw;
        }
    }

    public void PlayerHasPermission(int playerId, Board board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}
