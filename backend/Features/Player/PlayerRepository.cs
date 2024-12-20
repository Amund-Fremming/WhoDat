using Backend.Features.Database;
using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;
using System.Data;

namespace Backend.Features.Player;

public class PlayerRepository(AppDbContext context, ILogger<PlayerRepository> logger)
    : RepositoryBase<PlayerEntity, PlayerRepository>(logger, context), IPlayerRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<PlayerRepository> _logger = logger;

    public async Task<Result> DeletePlayer(int playerId)
    {
        try
        {
            var result = await GetById(playerId);
            if (result.IsError)
                return result.Error;

            _context.Player.Remove(result.Data);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return new Error(e, "Failed to delete user.");
        }
    }

    public async Task<Result<PlayerEntity>> GetPlayerByUsername(string username)
    {
        try
        {
            var user = await _context.Player.FirstAsync(p => p.Username == username);
            if (user == null)
                return new Error(new KeyNotFoundException("Username does not exist."), "Username does not exist");

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetPlayerByUsername)");
            return new Error(e, "Failed to get username.");
        }
    }

    public async Task<Result> Update(PlayerEntity player)
    {
        try
        {
            _context.Player.Update(player);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return new Error(e, "Failed to update password.");
        }
    }

    public async Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        try
        {
            return await _context.Player
                .Select(p => new PlayerDto(p.ID, p.Username, "", p.ImageUrl))
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return new Error(e, "Failed to get all players.");
        }
    }

    public async Task<Result> UsernameExist(string username)
    {
        try
        {
            bool usernameExist = await _context.Player
                .AnyAsync(p => p.Username == username);

            if (usernameExist)
                return new Error(new DuplicateNameException("Username exists"), "Username already exists.");

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DoesUsernameExist)");
            return new Error(e, "System error. Please try again later.");
        }
    }
}