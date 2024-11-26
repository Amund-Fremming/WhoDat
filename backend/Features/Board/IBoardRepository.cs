using Backend.Features.BoardCard;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public interface IBoardRepository : IRepository<BoardEntity>
{
    /// <summary>
    /// Sets the Chosen BoardCard on a board and saves to the database.
    /// </summary>
    /// <param name="board">Board to be set the chosen card on.</param>
    /// <param name="boardCard">The chosen BoardCard.</param>
    Task<Result> ChooseBoardCard(BoardEntity board, BoardCardEntity boardCard);

    /// <summary>
    /// Updates the number of active BoardCards left.
    /// </summary>
    /// <param name="board">Board to be set cards left on.</param>
    /// <param name="playersLeft">Number of active BoardCards left.</param>
    Task<Result> UpdateBoardCardsLeft(BoardEntity board, int playersLeft);
}