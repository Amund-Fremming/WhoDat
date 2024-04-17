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

    public async Task CreateBoardCards(int boardId, List<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (int cardId in cardIds)
                {
                    await _cardRepository.GetCardById(cardId);
                    await _boardcardRepository.CreateBoardCard(new BoardCard(boardId, cardId));
                }

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

    public async Task<bool> DeleteBoardCard(int boardCardId)
    {
        try
        {
            BoardCard boardCard = await _boardcardRepository.GetBoardCardById(boardCardId);

            return await _boardcardRepository.DeleteBoardCard(boardCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting BoardCard with id {boardCardId}. (BoardCardService)");
            throw;
        }
    }

    public async Task<bool> UpdateActive(int boardCardId, bool active)
    {
        try
        {
            BoardCard boardCard = await _boardcardRepository.GetBoardCardById(boardCardId);

            return await _boardcardRepository.UpdateActive(boardCard, active);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating BoardCard with id {boardCardId}. (BoardCardService)");
            throw;
        }
    }
}
