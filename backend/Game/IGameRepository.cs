using Enum;
using PlayerEntity;

namespace GameEntity;

public interface IGameRepository
{
    Task<Game> GetGameById(int gameId);

    Task<int> CreateGame(Game game, Player player);

    Task DeleteGame(Game game);

    Task JoinGame(Game game, Player player);

    Task LeaveGame(Game game);

    Task UpdateGameState(Game game, State state);

    Task<int> GetRecentGamePlayed(int playerId);

    Task UpdateGame(Game game);
}
