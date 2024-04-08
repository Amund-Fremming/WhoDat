namespace BoardEntity;

public interface IBoardService
{
    /* Basic CRUD */
    public Task<int> CreateBoard(Board board);
    public Task DeleteBoard(int boardId);

    /* Other */
    public Task ChooseCard(int boardId, int boardCardId);
    public Task UpdatePlayersLeft(int boardId, int activePlayers);
}
