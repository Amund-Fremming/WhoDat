namespace BoardCardEntity;

public interface IBoardCardService
{
    public Task CreateBoardCards(int playerId, int boardId, List<int> cardIds);
    public Task UpdateActive(int playerId, int boardCardId, bool active);
}
