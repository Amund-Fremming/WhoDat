namespace BoardCardEntity;

public interface IBoardCardService
{
    /* Basic CRUD */
    public Task CreateBoardCards(int boardId, List<int> cardIds);
    public Task<bool> DeleteBoardCard(int boardCardId);

    /* Other */
    public Task<bool> UpdateActive(int boardCardId, bool active);
}
