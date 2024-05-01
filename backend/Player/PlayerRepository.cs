using Data;
using Microsoft.EntityFrameworkCore;

namespace PlayerEntity;

public class PlayerRepository(AppDbContext context, ILogger<PlayerRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<PlayerRepository> _logger = logger;

    public async Task<Player> GetPlayerById(int playerId)
    {
        return await _context.Player
            .FindAsync(playerId) ?? throw new KeyNotFoundException($"Player with id {playerId}, does not exist!");
    }

    public async Task<int> CreatePlayer(Player player)
    {
        try
        {
            await _context.AddAsync(player);
            await _context.SaveChangesAsync();
            return player.PlayerID;
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error creating Player with id {player.PlayerID} .(PlayerRepository)");
            throw;
        }
    }

    public async Task DeletePlayer(Player player)
    {
        try
        {
            _context.Player.Remove(player);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error deleting Player with id {player.PlayerID} .(PlayerRepository)");
            throw;
        }
    }

    public async Task<Player> GetPlayerByUsername(string username)
    {
        return await _context.Player
            .FirstAsync(p => p.Username == username)
            ?? throw new KeyNotFoundException($"Username {username} does not exist. (AuthService)");
    }

    public async Task UpdateUsername(Player player, string newUsername)
    {
        try
        {
            player.Username = newUsername;
            _context.Player.Update(player);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error updating username for Player with id {player.PlayerID} .(PlayerRepository)");
            throw;
        }
    }

    public async Task UpdatePassword(Player player, string newPassword, string newSalt)
    {
        try
        {
            player.PasswordHash = newPassword;
            player.PasswordSalt = newSalt;

            _context.Player.Update(player);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error updating username for Player with id {player.PlayerID} .(PlayerRepository)");
            throw;
        }
    }

    public async Task DoesUsernameExist(string username)
    {
        bool usernameExist = await _context.Player
            .AnyAsync(p => p.Username == username);

        if (usernameExist)
            throw new ArgumentException("Username {username} already exists!");
    }
}
