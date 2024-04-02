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

    public Task DeleteGame(int gameId)
    {

    }

    public Task<int> JoinGameById(int gameId)
    {

    }

    public Task<int> UpdateGameState(State state)
    {

    }

    public Task UpdateCurrentPlayerTurn(string playerId)
    {

    }

}
