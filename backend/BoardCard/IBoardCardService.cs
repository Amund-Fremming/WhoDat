namespace BoardCardEntity;

public interface IBoardCardService
{
    /* Basic CRUD */
    public Task<int> CreateBoardCard(BoardCard boardCard);
    public Task<bool> DeleteBoardCard(int boardCardId);

    /* Other */
    public Task<bool> UpdateActive(int boardCardId, bool active);
}
