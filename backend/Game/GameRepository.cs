using Data;

namespace GameEntity;

public class GameRepository(AppDbContext context, ILogger<GameRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GameRepository> _logger = logger;

    public async Task<Game> GetGameById(int gameId)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    async public Task<int> CreateGame(Game game)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    async public Task<bool> DeleteGame(int gameId)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    async public Task<bool> JoinGameById(int gameId, string playerId)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    async public Task<bool> LeaveGameById(int gameId, string playerId)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    async public Task<bool> UpdateGameState(int gameId, State state)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

    public async Task<bool> UpdateCurrentPlayerTurn(int gameId, int playerNumber)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {gameId} .(GameRepository)");
        }
    }

}
