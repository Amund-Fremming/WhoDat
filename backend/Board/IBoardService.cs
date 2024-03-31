namespace BoardEntity;

public interface IBoardService
{
    /* Basic CRUD */
    public Task<Board> GetBoardById(int boardId);
    public Task<int> CreateBoard(Board board);
    public Task DeleteBoard(int boardId);
    public Task UpdateBoard(Board board);

    /* Other */
    public Task ChooseCard(int cardId);
    public Task UpdatePlayersLeft(int cardId);
}
