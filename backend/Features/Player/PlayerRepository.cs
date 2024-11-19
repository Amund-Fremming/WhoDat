using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

public class PlayerRepository(AppDbContext context, ILogger<IPlayerRepository> logger) : IPlayerRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IPlayerRepository> _logger = logger;

    public async Task<Result<PlayerEntity>> GetPlayerById(int playerId)
    {
        var player = await _context.Player.FindAsync(playerId);
        if (player == null)
            return Result<PlayerEntity>.Failure("Player does not exist.");

        return Result<PlayerEntity>.Success(player);
    }

    public async Task<Result> CreatePlayer(PlayerEntity player)
    {
        try
        {
            await _context.AddAsync(player);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return Result.Failure(e, "Failed to create player. Please try again later.");
        }
    }

    public async Task<Result> DeletePlayer(int playerId)
    {
        try
        {
            var result = await GetPlayerById(playerId);
            if (!result.IsSuccess)
                return result.RemoveType();

            _context.Player.Remove(result.Data);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return Result.Failure(e, "Failed to delete user. Please try again later.");
        }
    }

    public async Task<Result<PlayerEntity>> GetPlayerByUsername(string username)
    {
        var user = await _context.Player.FirstAsync(p => p.Username == username);
        if (user == null)
            return Result<PlayerEntity>.Failure("Username does not exist");

        return user;
    }

    public async Task<Result> UpdateUsername(PlayerEntity player, string newUsername)
    {
        try
        {
            player.Username = newUsername;
            _context.Player.Update(player);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return Result.Failure(e, "Failed to update username. Please try again later.");
        }
    }

    public async Task<Result> UpdatePassword(PlayerEntity player, string newPassword, string newSalt)
    {
        try
        {
            player.PasswordHash = newPassword;
            player.PasswordSalt = newSalt;

            _context.Player.Update(player);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return Result.Failure(e, "Failed to update password. Please try again later.");
        }
    }

    public async Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        try
        {
            return await _context.Player
                .Select(p => new PlayerDto() { PlayerID = p.PlayerID, Username = p.Username, ImageUrl = p.ImageUrl })
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return Result<IEnumerable<PlayerDto>>.Failure(e, "Failed to retrieve players. Please try again later.\r\n");
        }
    }

    public async Task<Result> DoesUsernameExist(string username)
    {
        try
        {
            bool usernameExist = await _context.Player
        .AnyAsync(p => p.Username == username);

            if (usernameExist)
                return Result.Failure("Username exists");

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DoesUsernameExist)");
            return Result.Failure(e, "Failed to check username existence. Please try again later.");
        }
    }
}