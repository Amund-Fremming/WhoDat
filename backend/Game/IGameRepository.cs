using Enum;
using PlayerEntity;

namespace GameEntity;

public interface IGameRepository
{
    /// <summary>
    /// Get a Gallery corresponding to the given id.
    /// </summary>
    /// <param name="galleryId">The id for the Gallery.</param>
    /// <returns>The Gallery asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Gallery does not exist.</exception>
    Task<Game> GetGameById(int gameId);

    /// <summary>
    /// Creates a new game with the specified game details and initiates it with the provided player.
    /// </summary>
    /// <param name="game">The game object containing details about the game to be created.</param>
    /// <param name="player">The player who is initiating the game.</param>
    Task<int> CreateGame(Game game, Player player);

    /// <summary>
    /// Deletes the specified game from the system.
    /// </summary>
    /// <param name="game">The game object representing the game to be deleted.</param>
    Task DeleteGame(Game game);

    /// <summary>
    /// Allows a player to join an existing game.
    /// </summary>
    /// <param name="game">The game object representing the game to join.</param>
    /// <param name="player">The player who wants to join the game.</param>
    Task JoinGame(Game game, Player player);

    /// <summary>
    /// Allows a player to leave an existing game.
    /// </summary>
    /// <param name="game">The game object representing the game to leave.</param>
    Task LeaveGame(Game game);

    /// <summary>
    /// Updates the state of the specified game.
    /// </summary>
    /// <param name="game">The game object representing the game to update.</param>
    /// <param name="state">The new state to be applied to the game.</param>
    Task UpdateGameState(Game game, State state);

    /// <summary>
    /// Retrieves the most recent game played by the specified player.
    /// </summary>
    /// <param name="playerId">The ID of the player whose recent game is to be retrieved.</param>
    /// <returns>Returns the ID of the most recent game played by the player.</returns>
    Task<int> GetRecentGamePlayed(int playerId);

    /// <summary>
    /// Updates the details of the specified game.
    /// </summary>
    /// <param name="game">The game object containing the updated details of the game.</param>
    Task UpdateGame(Game game);
}
