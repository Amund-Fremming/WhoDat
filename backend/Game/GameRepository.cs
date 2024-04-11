using Data;
using PlayerEntity;
using Enum;

namespace GameEntity;

public class GameRepository(AppDbContext context, ILogger<GameRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GameRepository> _logger = logger;

    public async Task<Game> GetGameById(int gameId)
    {
        return await _context.Game
            .FindAsync(gameId) ?? throw new KeyNotFoundException($"Game with id {gameId}, does not exist!");
    }

    async public Task<int> CreateGame(Game game, Player player)
    {
        try
        {
            game.PlayerOneID = player.PlayerID;
            game.PlayerOne = player;
            await _context.AddAsync(game);

            await _context.SaveChangesAsync();
            return game.GameID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Game with id {game.GameID} .(GameRepository)");
            return -1;
        }
    }

    async public Task<bool> DeleteGame(Game game)
    {
        try
        {
            _context.Game.Remove(game);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting Game with id {game.GameID} .(GameRepository)");
            return false;
        }
    }

    async public Task<bool> JoinGame(Game game, Player player)
    {
        try
        {
            game.PlayerTwoID = player.PlayerID;
            game.PlayerTwo = player;
            _context.Game.Update(game);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error joining Game with id {game.GameID} .(GameRepository)");
            return false;
        }
    }

    async public Task<bool> LeaveGame(Game game, int playerNumber)
    {
        try
        {
            if (playerNumber == 1)
            {
                game.PlayerOneID = -1;
                game.PlayerOne = null;
                game.State = State.P2_WON;
            }

            if (playerNumber == 2)
            {
                game.PlayerTwoID = -1;
                game.PlayerTwo = null;
                game.State = State.P1_WON;
            }

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error leaving Game with id {game.GameID} .(GameRepository)");
            return false;
        }
    }

    async public Task<bool> UpdateGameState(Game game, State state)
    {
        try
        {
            game.State = state;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating Game State with id {game.GameID} .(GameRepository)");
            return false;
        }
    }

    public async Task<bool> UpdateCurrentPlayerTurn(Game game, int playerNumber)
    {
        try
        {
            game.CurrentPlayer = playerNumber;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating current player in Game with id {game.GameID} .(GameRepository)");
            return false;
        }
    }
}
