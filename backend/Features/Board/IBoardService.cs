using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public interface IBoardService
{
    /// <summary>
    /// Deletes a instance of a board for a player.
    /// </summary>
    /// <param name="playerId">The id of the player to delete a board for.</param>
    /// <param name="boardId">The id of the board to delete.</param>
    public Task<Result> DeleteBoard(int playerId, int boardId);

    /// <summary>
    /// Sets the players choosen card on the board to play the game with.
    /// </summary>
    /// <param name="playerId">The id of the player to choose a boardcard for.</param>
    /// <param name="boardId">The id of the board to set the choosen card on.</param>
    /// <param name="gameId">The id of the game to set the choosen card on.</param>
    /// <param name="boardCardId">The id of the boardcard choosen.</param>
    /// <returns>The new state of the game.</returns>
    public Task<Result<GameState>> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId);

    /// <summary>
    /// Updates the active boardcards left on the players board.
    /// </summary>
    /// <param name="playerId">The id of the player to update boardcards left for.</param>
    /// <param name="boardId">The board we are updating on.</param>
    /// <param name="activePlayers">The number of currently active boardcards on the board.</param>
    public Task<Result> UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers);

    public Task<Result<BoardEntity>> GetBoardWithBoardCards(int playerId, int gameId);

    /// <summary>
    /// Takes in a guess for a boardcard, and checks if the guess is the same boardcard as the other player has selected on their board.
    /// </summary>
    /// <param name="playerId">The id of the player that are guessing.</param>
    /// <param name="gameId">The id of the game we are guesing in.</param>
    /// <param name="guessedBoardCardId">The id of the boardcard the player is guessing.</param>
    /// <returns>The new state of the game.</returns>
    public Task<Result<GameState>> GuessBoardCard(int playerId, int gameId, int guessedBoardCardId);
}