using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public class GameRepository(AppDbContext context, ILogger<IGameRepository> logger) : IGameRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IGameRepository> _logger = logger;

    public async Task<Result<GameEntity>> GetGameById(int gameId)
    {
        var game = await _context.Game.FindAsync(gameId);
        if (game == null)
            return new Error(new KeyNotFoundException("Game id does not exist."), "Game does not exist.");

        return game;
    }

    public async Task<Result<int>> CreateGame(GameEntity game, PlayerEntity player)
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
            _logger.LogError(e, "(CreateGame)");
            return new Error(e, "Failed creating game.");
        }
    }

    public async Task<Result> DeleteGame(GameEntity game)
    {
        try
        {
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteGame)");
            return new Error(e, "Failed to delete game.");
        }
    }

    public async Task<Result> JoinGame(GameEntity game, PlayerEntity player)
    {
        try
        {
            game.PlayerTwoID = player.PlayerID;
            game.PlayerTwo = player;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(JoinGame)");
            return new Error(e, "Failed to join game");
        }
    }

    public async Task<Result> LeaveGame(GameEntity game)
    {
        try
        {
            game.GameState = GameState.PLAYER_LEFT;

            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to leave game.");
            return new Error(e, "Failed to leave game.");
        }
    }

    public async Task<Result> UpdateGameState(GameEntity game, GameState state)
    {
        try
        {
            game.GameState = state;
            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateGameState)");
            return new Error(e, "Failed to update gamestate.");
        }
    }

    public async Task<Result<int>> GetRecentGamePlayed(int playerId)
    {
        try
        {
            return await _context.Game
                .Where(g => g.PlayerOneID == playerId || g.PlayerTwoID == playerId)
                .MaxAsync(g => g.GameID);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetRecentGamePlayed)");
            return new Error(e, "Failed to get recent game played.");
        }
    }

    public async Task<Result> UpdateGame(GameEntity game)
    {
        try
        {
            _context.Game.Update(game);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateGame)");
            return new Error(e, "Failed to update game.");
        }
    }
}