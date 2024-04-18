namespace BoardEntity;

public interface IBoardService
{
    public Task<int> CreateBoard(int playerId, Board board);
    public Task DeleteBoard(int playerId, int boardId);
    public Task ChooseCard(int boardId, int boardCardId);
    public Task UpdatePlayersLeft(int boardId, int activePlayers);
}
