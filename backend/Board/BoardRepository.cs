using Data;

namespace BoardEntity;

public class BoardRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;

    public Task<int> CreateBoard(Board board);
    public Task DeleteBoard(int boardId);

    /* Other */
    public Task ChooseCard(int cardId);
    public Task UpdatePlayersLeft(int cardId);

    public async Task<Board> GetBoardById(int boardId)
    {
        return _context.
    }
}
