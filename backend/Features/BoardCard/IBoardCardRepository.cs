using Backend.Features.Shared.Common;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.BoardCard;

public interface IBoardCardRepository : IRepository<BoardCardEntity>
{
    Task<Result> CreateBoardCards(IEnumerable<BoardCardEntity> boardCards);

    Task<Result> UpdateBoardCards( IEnumerable<BoardCardEntity> boardCards);

    Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int boardId);
}