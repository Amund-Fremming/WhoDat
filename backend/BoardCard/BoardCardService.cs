using BoardEntity;
using CardEntity;

namespace BoardCardEntity;

public class BoardCardService(ILogger<BoardCardService> logger, BoardCardRepository boardcardRepository, BoardRepository boardRepository, CardRepository cardRepository) : IBoardCardService
{
    public readonly ILogger<BoardCardService> _logger = logger;
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly CardRepository _cardRepository = cardRepository;

    public async Task<int> CreateBoardCard(BoardCard boardCard)
    {
        try
        {
            await _cardRepository.GetCardById(boardCard.CardID);

            return await _boardcardRepository.CreateBoardCard(boardCard);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating BoardCard with id {boardCard.BoardCardID}. (BoardCardService)");
            throw;
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
