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
            _logger.LogError(e, $"Error creating Player with id {player.PlayerID} .(PlayerRepository)");
            return -1;
        }
    }

    public async Task<bool> DeletePlayer(Player player)
    {
        try
        {
            _context.Player.Remove(player);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting Player with id {player.PlayerID} .(PlayerRepository)");
            return false;
        }
    }

    public async Task<Player> GetPlayerByUsername(string username)
    {
        return await _context.Player
            .FirstAsync(p => p.Username == username)
            ?? throw new KeyNotFoundException($"Username {username} does not exist. (AuthService)");
    }

    public async Task<bool> UpdateUsername(Player player, string newUsername)
    {
        try
        {
            player.Username = newUsername;
            _context.Player.Update(player);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating username for Player with id {player.PlayerID} .(PlayerRepository)");
            return false;
        }
    }

    public async Task<bool> DoesUsernameExist(string username)
    {
        return await _context.Player
            .AnyAsync(p => p.Username == username);
    }
}
