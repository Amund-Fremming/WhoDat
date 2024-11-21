using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

public class PlayerRepository(AppDbContext context, ILogger<IPlayerRepository> logger, IPasswordHasher<PlayerEntity> passwordHasher) : IPlayerRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IPlayerRepository> _logger = logger;
    public readonly IPasswordHasher<PlayerEntity> _passwordHasher = passwordHasher;

    public async Task<Result<PlayerEntity>> GetPlayerById(int playerId)
    {
        try
        {
            var player = await _context.Player.FindAsync(playerId);
            if (player == null)
                return (new KeyNotFoundException("Player id does not exist"), "Player does not exist.");

            return player;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetPlayerById)");
            return (e, "System error. Please try again later.");
        }
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
            return (e, "Failed to create player. Please try again later.");
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
            return (e, "Failed to delete user. Please try again later.");
        }
    }

    public async Task<Result<PlayerEntity>> GetPlayerByUsername(string username)
    {
        try
        {
            var user = await _context.Player.FirstAsync(p => p.Username == username);
            if (user == null)
                return (null, "Username does not exist");

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetPlayerByUsername)");
            return (e, "Failed to get username. Please try again later.");
        }
    }

    public async Task<Result> UpdateUsername(int playerId, string newUsername)
    {
        try
        {
            var result = await GetPlayerById(playerId);
            if (!result.IsSuccess)
                return result.RemoveType();

            var player = result.Data;
            player.Username = newUsername;
            _context.Player.Update(player);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return (e, "Failed to update username. Please try again later.");
        }
    }

    public async Task<Result> UpdatePassword(int playerId, string newPassword)
    {
        try
        {
            var result = await GetPlayerById(playerId);
            if (!result.IsSuccess)
                return result.RemoveType();

            var player = result.Data;
            player.PasswordHash = newPassword;
            player.PasswordSalt = GenerateSalt();

            _context.Player.Update(player);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return (e, "Failed to update password. Please try again later.");
        }
    }

    public async Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        try
        {
            return await _context.Player
                .Select(p => new PlayerDto(p.PlayerID, p.Username, "", p.ImageUrl))
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(PlayerRepository)");
            return (e, "Failed to get all players. Please try again later.");
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
            return (e, "System error. Please try again later.");
        }
    }

    public string GenerateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator.Fill(buffer);

        return Convert.ToBase64String(buffer);
    }
}