using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Game;

public class GameRepository(AppDbContext context, ILogger<IGameRepository> logger) : IGameRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IGameRepository> _logger = logger;

    public async Task<GameEntity> GetGameById(int gameId)
    {
        return await _context.Game
            .FindAsync(gameId) ?? throw new KeyNotFoundException($"Game with id {gameId}, does not exist!");
    }

    public async Task<int> CreateGame(GameEntity game, PlayerEntity player)
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

    public async Task DeleteGame(GameEntity game)
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

    public async Task JoinGame(GameEntity game, PlayerEntity player)
    {
        try
        {
            game.PlayerTwoID = player.PlayerID;
            game.PlayerTwo = player;

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

    public async Task LeaveGame(GameEntity game)
    {
        try
        {
            game.GameState = GameState.PLAYER_LEFT;

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

    public async Task UpdateGameState(GameEntity game, GameState state)
    {
        try
        {
            game.GameState = state;

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

    public async Task UpdateGame(GameEntity game)
    {
        try
        {
            _context.Game.Update(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error updating Game with id {game.GameID}. (GameRepository)");
            throw;
        }
    }
}