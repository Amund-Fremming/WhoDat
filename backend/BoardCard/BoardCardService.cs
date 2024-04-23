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

    public async Task UpdateActive(int playerId, int boardCardId, bool active)
    {
        try
        {
            BoardCard boardCard = await _boardcardRepository.GetBoardCardById(boardCardId);
            PlayerHasPermission(playerId, boardCard.Board!);

            await _boardcardRepository.UpdateActive(boardCard, active);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating BoardCard with id {boardCardId}. (BoardCardService)");
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
