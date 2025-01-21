using Backend.Features.BoardCard;
using Backend.Features.Shared.Common;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public interface IBoardRepository : IRepository<BoardEntity>
{
    Task<Result> ChooseBoardCard(BoardEntity board, BoardCardEntity boardCard);

    Task<Result> UpdateBoardCardsLeft(BoardEntity board, int playersLeft);

    Task<Result<BoardEntity>> GetBoardWithBoardCards(int boardId);
}