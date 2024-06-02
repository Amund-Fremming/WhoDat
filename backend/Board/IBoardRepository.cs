using BoardCardEntity;

namespace BoardEntity;

public interface IBoardRepository
{
    Task<Board> GetBoardById(int boardId);

    Task<int> CreateBoard(Board board);

    Task DeleteBoard(Board board);

    Task ChooseBoardCard(Board board, BoardCard boardCard);

    Task UpdateBoardCardsLeft(Board board, int playersLeft);
}

