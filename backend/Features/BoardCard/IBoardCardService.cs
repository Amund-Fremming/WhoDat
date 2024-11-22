using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.BoardCard;

public interface IBoardCardService
{
    /// <summary>
    /// Creates a collection of boardcards from a enumerable of card ids.
    /// </summary>
    /// <param name="playerId">The player creating the boardcards.</param>
    /// <param name="boardId">The game to create boardcards for.</param>
    /// <param name="cardIds">Card ids to create boardcards from.</param>
    /// <returns>The new game State.</returns>
    public Task<Result<GameState>> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds);

    /// <summary>
    /// Updates the activity for multiple boardcards at once.
    /// </summary>
    /// <param name="playerId">The player updating the boardcards.</param>
    /// <param name="boardId">The game where the boardcards are stored.</param>
    /// <param name="boardCardUpdates">A collection of boardcard ids and a active variable with their new activity.</param>
    /// <returns>The number of boardcard left on the board.</returns>
    public Task<Result> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates);

    /// <summary>
    /// Retrieves the boardcards from a board.
    /// </summary>
    /// <param name="playerId">The player retrieving the boardcards.</param>
    /// <param name="boardId">The board to retrieve the boardcards from.</param>
    /// <returns>A collection of boardcards.</returns>
    public Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int playerId, int boardId);
}