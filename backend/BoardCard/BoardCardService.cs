namespace BoardCardEntity;

public class BoardCardService(ILogger<BoardCardService> logger, BoardCardRepository boardcardRepository) : IBoardCardService
{
    public readonly ILogger<BoardCardService> _logger = logger;
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;

    public Task<int> CreateBoardCard(BoardCard boardCard)
    {
        try
        {

        }
        catch (Exception)
        {
            // ADD HANDLING
            // ADD LOGGING
            _logger.LogError($"Error while creating BoardCard with id {boardId}. (BoardCardService)");
            throw;
        }

    }

    public Task<bool> DeleteBoardCard(int boardCardId)
    {
        try
        {

        }
        catch (Exception)
        {
            // ADD HANDLING
            // ADD LOGGING
            throw;
        }
    }

    public Task<bool> UpdateActive(bool active)
    {
        try
        {

        }
        catch (Exception)
        {
            // ADD HANDLING
            // ADD LOGGING
            throw;
        }
    }
}
