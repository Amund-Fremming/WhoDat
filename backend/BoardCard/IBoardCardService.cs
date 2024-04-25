namespace BoardCardEntity;

public interface IBoardCardService
{
    public Task CreateBoardCards(int playerId, int boardId, IEnumerable<int> cardIds);
    public Task UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates);
}
