using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public interface IBoardService
{
    public Task<Result> DeleteBoard(int playerId, int boardId);

    public Task<Result<GameState>> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId);

    public Task<Result> UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers);

    public Task<Result<BoardEntity>> GetBoardWithBoardCards(int playerId, int gameId);

    public Task<Result<GameState>> GuessBoardCard(int playerId, int gameId, int guessedBoardCardId);
}