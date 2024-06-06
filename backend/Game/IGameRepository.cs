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

    Task<int> CreateGame(Game game, Player player);

    Task DeleteGame(Game game);

    Task JoinGame(Game game, Player player);

    Task LeaveGame(Game game);

    Task UpdateGameState(Game game, State state);

    Task<int> GetRecentGamePlayed(int playerId);

    Task UpdateGame(Game game);
}
