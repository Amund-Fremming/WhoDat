using Backend.Features.Player;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public interface IGameRepository : IRepository<GameEntity>
{
    /// <summary>
    /// Allows a player to join an existing game.
    /// </summary>
    /// <param name="game">The game object representing the game to join.</param>
    /// <param name="player">The player who wants to join the game.</param>
    Task<Result> JoinGame(GameEntity game, PlayerEntity player);

    /// <summary>
    /// Allows a player to leave an existing game.
    /// </summary>
    /// <param name="game">The game object representing the game to leave.</param>
    Task<Result> LeaveGame(GameEntity game);

    /// <summary>
    /// Updates the state of the specified game.
    /// </summary>
    /// <param name="game">The game object representing the game to update.</param>
    /// <param name="state">The new state to be applied to the game.</param>
    Task<Result> UpdateGameState(GameEntity game, GameState state);

    /// <summary>
    /// Retrieves the most recent game played by the specified player.
    /// </summary>
    /// <param name="playerId">The ID of the player whose recent game is to be retrieved.</param>
    /// <returns>Returns the ID of the most recent game played by the player.</returns>
    Task<Result<int>> GetRecentGamePlayed(int playerId);

    /// <summary>
    /// Updates the details of the specified game.
    /// </summary>
    /// <param name="game">The game object containing the updated details of the game.</param>
    Task<Result> UpdateGame(GameEntity game);
}