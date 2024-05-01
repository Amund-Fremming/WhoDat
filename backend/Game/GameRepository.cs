using Data;
using PlayerEntity;
using Enum;
using Microsoft.EntityFrameworkCore;

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

    public async Task<int> CreateGame(Game game, Player player)
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
            // TODO - more exceptions
            _logger.LogError(e, $"Error creating Game with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task DeleteGame(Game game)
    {
        try
        {
            _context.Game.Remove(game);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error deleting Game with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task JoinGame(Game game, Player player)
    {
        try
        {
            game.PlayerTwoID = player.PlayerID;
            game.PlayerTwo = player;
            game.State = State.READY;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error joining Game with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task LeaveGame(Game game)
    {
        try
        {
            game.State = State.FINISHED;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error leaving Game with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task UpdateGameState(Game game, State state)
    {
        try
        {
            game.State = state;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error updating Game State with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task UpdateCurrentPlayerTurn(Game game, int playerNumber)
    {
        try
        {
            game.CurrentPlayer = playerNumber;
            game.State = State.TURN_STARTED;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error updating current player in Game with id {game.GameID} .(GameRepository)");
            throw;
        }
    }

    public async Task<int> GetRecentGamePlayed(int playerId)
    {
        try
        {
            return await _context.Game
                .Where(g => g.PlayerOneID == playerId || g.PlayerTwoID == playerId)
                .MaxAsync(g => g.GameID);
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error while getting players recent Game with PlayerID {playerId}. (GameRepository)");
            throw;
        }
    }
}
