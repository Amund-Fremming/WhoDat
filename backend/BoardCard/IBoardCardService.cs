namespace BoardCardEntity;

public interface IBoardCardService
{
    public Task CreateBoardCards(int playerId, int boardId, IEnumerable<int> cardIds);
    public Task<int> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates);
    public Task<IEnumerable<BoardCard>> GetBoardCardsFromBoard(int playerId, int boardId);
}
