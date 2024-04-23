using Enum;

namespace GameEntity;

public interface IGameService
{
    public Task<int> CreateGame(int playerId, Game game);
    public Task DeleteGame(int playerId, int gameId);
    public Task JoinGameById(int playerId, int gameId);
    public Task LeaveGameById(int playerId, int gameId, int playerNumber);
    public Task UpdateGameState(int playerId, int gameId, State state);
    public Task UpdateCurrentPlayerTurn(int playerId, int gameId, int playerNumber);
}
