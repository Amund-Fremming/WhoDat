namespace BoardCardEntity;

public interface IBoardCardService
{
    public Task CreateBoardCards(int boardId, List<int> cardIds);
    public Task DeleteBoardCard(int boardCardId);
    public Task UpdateActive(int boardCardId, bool active);
}
