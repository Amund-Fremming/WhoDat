using Enum;

namespace GameEntity;

public interface IGameService
{
    /* Basic Crud */
    public Task<int> CreateGame(Game game);
    public Task<bool> DeleteGame(int gameId);

    /* Other */
    public Task<bool> JoinGameById(int gameId, string playerId);
    public Task<bool> LeaveGameById(int gameId, string playerId);
    public Task<bool> UpdateGameState(int gameId, State state);
    public Task<bool> UpdateCurrentPlayerTurn(int gameId, string playerNumber);
}
