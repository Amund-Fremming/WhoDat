using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public interface IGameService
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="playerId">The player creating the game.</param>
    /// <param name="gameState">The game state to be created</param>
    /// <returns>The id of the game created.</returns>
    public Task<Result<int>> CreateGame(int playerId, GameState gameState);

    /// <summary>
    /// Deletes a existing game.
    /// </summary>
    /// <param name="playerId">The player deleting a game.</param>
    /// <param name="gameId">The game to delete.</param>
    public Task<Result> DeleteGame(int playerId, int gameId);

    /// <summary>
    /// Adds a player to a game.
    /// </summary>
    /// <param name="playerId">The player to be added to the game.</param>
    /// <param name="gameId">The game to be added in.</param>
    /// <returns>The game the player was added to.</returns>
    public Task<Result<GameEntity>> JoinGameById(int playerId, int gameId);

    /// <summary>
    /// Removes a player from a game.
    /// </summary>
    /// <param name="playerId">The player to be removed.</param>
    /// <param name="gameId">The game to be removed from.</param>
    public Task<Result> LeaveGameById(int playerId, int gameId);

    /// <summary>
    /// Updates the game state.
    /// </summary>
    /// <param name="playerId">The player updating the game state.</param>
    /// <param name="gameId">The game to update the state for.</param>
    /// <param name="state">The new state to be updated to.</param>
    public Task<Result> UpdateGameState(int playerId, int gameId, GameState state);

    /// <summary>
    /// Fetches the most recent game a player has player based on the id of the games.
    /// Only used for OnDisconnect in the GameHub for removing the player from the group.
    /// </summary>
    /// <param name="playerId">The player to get recent games from.</param>
    /// <returns>The id of the most recent game for the player.</returns>
    public Task<Result<int>> GetRecentGamePlayed(int playerId);

    /// <summary>
    /// Updates the game state so a player can start playing.
    /// </summary>
    /// <param name="playerId">The player starting the game.</param>
    /// <param name="gameId">The game to start.</param>
    /// <returns>The new updated state.</returns>
    public Task<Result<GameState>> StartGame(int playerId, int gameId);
}