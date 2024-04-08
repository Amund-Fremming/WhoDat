namespace BoardCardEntity;

public interface IBoardCardService
{
    /* Basic CRUD */
    public Task<BoardCard> GetBoardCardById(int boardCardId);
    public Task<int> CreateBoardCard(BoardCard boardCard);
    public Task<bool> DeleteBoardCard(int boardCardId);

    /* Other */
    public Task<bool> UpdateActive(bool active);
}
