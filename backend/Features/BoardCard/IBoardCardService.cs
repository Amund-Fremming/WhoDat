using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.BoardCard;

public interface IBoardCardService
{
    public Task<Result<GameState>> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds);

    public Task<Result<int>> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<int> activeBoardCardIds);

    public Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int playerId, int boardId);
}