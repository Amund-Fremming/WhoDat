namespace BoardCardEntity;

public interface IBoardCardService
{
    /* Basic CRUD */
    public Task<BoardCard> GetBoardCardById(int boardId);
    public Task<int> CreateBoardCard(BoardCard boardCard);
    public Task DeleteBoardCard(int boardCardId);
}
