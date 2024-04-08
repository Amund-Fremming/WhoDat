using Enum;

namespace GameEntity;

public class GameService(GameRepository gameRepository) : IGameService
{
    public readonly GameRepository _gameRepository = gameRepository;

    public Task<Game> GetGameById(int gameId)
    {

    }

    public Task<int> CreateGame(Game game)
    {

    }

    public Task<bool> DeleteGame(int gameId)
    {

    }

    public Task<bool> JoinGameById(int gameId, string playerId)
    {

    }

    public Task<bool> LeaveGameById(int gameId, string playerId)
    {

    }

    public Task<bool> UpdateGameState(int gameId, State state)
    {

    }

    public Task<bool> UpdateCurrentPlayerTurn(int gameId, string playerId)
    {

    }

}
