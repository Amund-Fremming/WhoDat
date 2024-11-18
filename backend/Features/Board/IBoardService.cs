using RaptorProject.Features.Shared.Enums;

namespace BoardEntity;

public interface IBoardService
{
    /// <summary>
    /// Creates a new instance of a board for a player in a game.
    /// </summary>
    /// <param name="playerId">The id of the player to create a board for.</param>
    /// <param name="gameId">The id of the game to create a board for.</param>
    /// <returns>The id of the board created</returns>
    /// <exception cref="KeyNotFoundException">Throws if the player or game does not exist.</exception>
    public Task<int> CreateBoard(int playerId, int gameId);

    /// <summary>
    /// Deletes a instance of a board for a player.
    /// </summary>
    /// <param name="playerId">The id of the player to delete a board for.</param>
    /// <param name="boardId">The id of the board to delete.</param>
    /// <exception cref="KeyNotFoundException">Throws if the board does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player does not own the board.</exception>
    public Task DeleteBoard(int playerId, int boardId);

    /// <summary>
    /// Sets the players choosen card on the board to play the game with.
    /// </summary>
    /// <param name="playerId">The id of the player to choose a boardcard for.</param>
    /// <param name="boardId">The id of the board to set the choosen card on.</param>
    /// <param name="gameId">The id of the game to set the choosen card on.</param>
    /// <param name="boardCardId">The id of the boardcard choosen.</param>
    /// <returns>The new state of the game.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the board or boardcard does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player does not own the board or is in the game.</exception>
    /// <exception cref="InvalidOperationException">Throws if the state does not allow for choosing cards.</exception>
    public Task<State> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId);

    /// <summary>
    /// Updates the active boardcards left on the players board.
    /// </summary>
    /// <param name="playerId">The id of the player to update boardcards left for.</param>
    /// <param name="boardId">The board we are updating on.</param>
    /// <param name="activePlayers">The number of currently active boardcards on the board.</param>
    /// <exception cref="KeyNotFoundException">Throws if the board does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player does not own the board.</exception>
    public Task UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers);

    // TODO
    public Task<Board> GetBoardWithBoardCards(int playerId, int gameId);

    /// <summary>
    /// Takes in a guess for a boardcard, and checks if the guess is the same boardcard as the other player has selected on their board.
    /// </summary>
    /// <param name="playerId">The id of the player that are guessing.</param>
    /// <param name="gameId">The id of the game we are guesing in.</param>
    /// <param name="guessedBoardCardId">The id of the boardcard the player is guessing.</param>
    /// <returns>The new state of the game.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the game or boardcard does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player does not own the board.</exception>
    public Task<State> GuessBoardCard(int playerId, int gameId, int guessedBoardCardId);
}
