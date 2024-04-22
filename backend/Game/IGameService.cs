using Enum;

namespace GameEntity;

public interface IGameService
{
    public Task<int> CreateGame(int playerId, Game game);
    public Task<bool> DeleteGame(int playerId, int gameId);
    public Task<bool> JoinGameById(int playerId, int gameId);
    public Task<bool> LeaveGameById(int playerId, int gameId, int playerNumber);
    public Task<bool> UpdateGameState(int playerId, int gameId, State state);
    public Task<bool> UpdateCurrentPlayerTurn(int playerId, int gameId, int playerNumber);
}
