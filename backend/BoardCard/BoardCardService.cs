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

    public async Task CreateBoardCards(int playerId, int boardId, List<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                foreach (int cardId in cardIds)
                {
                    await _cardRepository.GetCardById(cardId);
                    await _boardcardRepository.CreateBoardCard(new BoardCard(boardId, cardId));
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while creating BoardCards for board with id {boardId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        try
        {
            // TODO
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            foreach (BoardCardUpdate update in boardCardUpdates)
            {
                BoardCard boardCard = await _boardcardRepository.GetBoardCardById(update.BoardCardID);
                await _boardcardRepository.UpdateActive(boardCard, update.Active);
            }
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
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
