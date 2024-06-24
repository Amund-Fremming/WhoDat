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
    /// Creates a new game.
    /// </summary>
    /// <param name="game">The game object to be created in the db.</param>
    /// <param name="game">The player object to be created in the db.</param>
    /// <returns>The id for the created game.</returns>
    Task<int> CreateGame(Game game, Player player);

    /// <summary>
    /// Deletes a existing game.
    /// </summary>
    /// <param name="game">The game object to be deleted in the db.</param>
    Task DeleteGame(Game game);

    /// <summary>
    /// Adds a player from a existing game.
    /// </summary>
    /// <param name="game">The game object to add the player to in the db.</param>
    /// <param name="player">The player to be added to the db.</param>
    Task JoinGame(Game game, Player player);

    /// <summary>
    /// Removes the player from the game, sets a new state and updates.
    /// </summary>
    /// <param name="game">The game object to update and set a new state for.</param>
    Task LeaveGame(Game game);

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">The game object to be created in the db.</param>
    /// <param name="game">The player object to be created in the db.</param>
    /// <returns>The id for the created game.</returns>
    Task UpdateGameState(Game game, State state);

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">The game object to be created in the db.</param>
    /// <param name="game">The player object to be created in the db.</param>
    /// <returns>The id for the created game.</returns>
    Task<int> GetRecentGamePlayed(int playerId);

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">The game object to be created in the db.</param>
    /// <param name="game">The player object to be created in the db.</param>
    /// <returns>The id for the created game.</returns>
    Task UpdateGame(Game game);
}
