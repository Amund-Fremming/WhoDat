namespace BoardEntity;

public interface IBoardService
{
    /* Basic CRUD */
    public Task<Board?> GetBoardById(int boardId);
    public Task<int> CreateBoard(Board board);
    public Task<bool> DeleteBoard(int boardId);

    /* Other */
    public Task<bool> ChooseCard(int cardId);
    public Task<bool> UpdatePlayersLeft(int cardId);
}
