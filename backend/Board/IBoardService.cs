using Enum;

namespace BoardEntity;

public interface IBoardService
{
    public Task<int> CreateBoard(int playerId, int gameId);
    public Task DeleteBoard(int playerId, int boardId);
    public Task<State> ChooseBoardCard(int playerId, int boardId, int boardCardId);
    public Task UpdatePlayersLeft(int playerId, int boardId, int activePlayers);
    public Task<Board> GetBoardWithBoardCards(int playerId, int gameId);
    public Task<State> GuessBoardCard(int playerId, int gameId, int guessedBoardCardId);
}
