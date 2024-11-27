using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public class GameRepository(AppDbContext context, ILogger<GameRepository> logger)
    : RepositoryBase<GameEntity, GameRepository>(logger, context), IGameRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<GameRepository> _logger = logger;

    public async Task<Result> JoinGame(GameEntity game, PlayerEntity player)
    {
        try
        {
            game.PlayerTwoID = player.ID;
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
                .MaxAsync(g => g.ID);
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