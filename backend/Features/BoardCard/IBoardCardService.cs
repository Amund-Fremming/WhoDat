using RaptorProject.Features.Shared.Enums;

namespace BoardCardEntity;

public interface IBoardCardService
{
    /// <summary>
    /// Creates a collection of boardcards from a enumerable of card ids.
    /// </summary>
    /// <param name="playerId">The player creating the boardcards.</param>
    /// <param name="boardId">The game to create boardcards for.</param>
    /// <param name="cardIds">Card ids to create boardcards from.</param>
    /// <returns>The new game State.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the game or board does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the game or board.</exception>
    public Task<State> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds);

    /// <summary>
    /// Updates the activity for multiple boardcards at once.
    /// </summary>
    /// <param name="playerId">The player updating the boardcards.</param>
    /// <param name="boardId">The game where the boardcards are stored.</param>
    /// <param name="boardCardUpdates">A collection of boardcard ids and a active variable with their new activity.</param>
    /// <returns>The number of boardcard left on the board.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the board does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the game or board.</exception>
    public Task<int> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates);

    /// <summary>
    /// Retrieves the boardcards from a board.
    /// </summary>
    /// <param name="playerId">The player retrieving the boardcards.</param>
    /// <param name="boardId">The board to retrieve the boardcards from.</param>
    /// <returns>A collection of boardcards.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the board does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the board.</exception>
    public Task<IEnumerable<BoardCard>> GetBoardCardsFromBoard(int playerId, int boardId);

}
