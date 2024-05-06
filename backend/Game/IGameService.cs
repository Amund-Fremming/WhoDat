using Enum;

namespace GameEntity;

public interface IGameService
{
    public Task<int> CreateGame(int playerId, Game game);
    public Task DeleteGame(int playerId, int gameId);
    public Task<Game> JoinGameById(int playerId, int gameId);
    public Task LeaveGameById(int playerId, int gameId);
    public Task UpdateGameState(int playerId, int gameId, State state);
    public Task<int> GetRecentGamePlayed(int playerId);
}
