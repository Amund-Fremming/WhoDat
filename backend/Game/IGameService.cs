using Enum;

namespace GameEntity;

public interface IGameService
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="playerId">The player creating the game.</param>
    /// <param name="game">The game to be created</param>
    /// <returns>The id of the game created.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the player or game does not exist.</exception>
    public Task<int> CreateGame(int playerId, Game game);
    
    /// <summary>
    /// Deletes a existing game.
    /// </summary>
    /// <param name="playerId">The player deleting a game.</param>
    /// <param name="gameId">The game to delete.</param>
    /// <exception cref="KeyNotFoundException">Throws if the game does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the game.</exception>
    public Task DeleteGame(int playerId, int gameId);

    /// <summary>
    /// Adds a player to a game.
    /// </summary>
    /// <param name="playerId">The player to be added to the game.</param>
    /// <param name="gameId">The game to be added in.</param>
    /// <returns>The game the player was added to.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the game or player does not exist.</exception>
    /// <exception cref="GameFullException">Throws if the game already has to players added.</exception>
    public Task<Game> JoinGameById(int playerId, int gameId);
    
    /// <summary>
    /// Removes a player from a game.
    /// </summary>
    /// <param name="playerId">The player to be removed.</param>
    /// <param name="gameId">The game to be removed from.</param>
    /// <exception cref="KeyNotFoundException">Throws if the game does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the game.</exception>
    public Task LeaveGameById(int playerId, int gameId);
    
    /// <summary>
    /// Updates the game state.
    /// </summary>
    /// <param name="playerId">The player updating the game state.</param>
    /// <param name="gameId">The game to update the state for.</param>
    /// <param name="state">The new state to be updated to.</param>
    /// <exception cref="KeyNotFoundException">Throws if the game does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the game.</exception>
    public Task UpdateGameState(int playerId, int gameId, State state);

    /// <summary>
    /// Fetches the most recent game a player has player based on the id of the games.
    /// Only used for OnDisconnect in the GameHub for removing the player from the group.
    /// </summary>
    /// <param name="playerId">The player to get recent games from.</param>
    /// <returns>The id of the most recent game for the player.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the player does not exist.</exception>
    public Task<int> GetRecentGamePlayed(int playerId);
}
