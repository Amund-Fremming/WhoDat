using Enum;

namespace GameEntity;

public interface IGameService
{
    /* Basic Crud */
    public Task<Game> GetGameById(int gameId);
    public Task<int> CreateGame(Game game);
    public Task DeleteGame(int gameId);

    /* Other */
    public Task<int> JoinGameById(int gameId);
    public Task<int> UpdateGameState(State state);
    public Task UpdateCurrentPlayerTurn(string playerId);
}
