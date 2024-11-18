namespace PlayerEntity;

public class PlayerService(ILogger<IPlayerService> logger, IPlayerRepository playerRepository, IPasswordHasher<Player> passwordHasher) : IPlayerService
{
    public readonly ILogger<IPlayerService> _logger = logger;
    public readonly IPlayerRepository _playerRepository = playerRepository;
    public readonly IPasswordHasher<Player> _passwordHasher = passwordHasher;

    public async Task<int> CreatePlayer(Player player)
    {
        try
        {
            await _playerRepository.DoesUsernameExist(player.Username);

            return await _playerRepository.CreatePlayer(player);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while creating Player with id {player.PlayerID}. (PlayerService)");
            throw;
        }
    }

    public async Task DeletePlayer(int playerId)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);

            await _playerRepository.DeletePlayer(player);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while deleting Player with id {playerId}. (PlayerService)");
            throw;
        }
    }

    public async Task UpdateUsername(int playerId, string newUsername)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);
            await _playerRepository.DoesUsernameExist(newUsername);

            await _playerRepository.UpdateUsername(player, newUsername);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while updating username for Player with id {playerId}. (PlayerService)");
            throw;
        }
    }

    public async Task UpdatePassword(int playerId, string newPassword)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);

            string newSalt = GenerateSalt();
            string saltedPassword = newPassword + newSalt;
            string hashedPassword = _passwordHasher.HashPassword(player, saltedPassword);

            await _playerRepository.UpdatePassword(player, hashedPassword, newSalt);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while updating username for Player with id {playerId}. (PlayerService)");
            throw;
        }
    }

    public async Task<IEnumerable<PlayerDto>> GetAllPlayers()
    {
        try
        {
            return await _playerRepository.GetAllPlayers();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error getting all players. (PlayerService)");
            throw;
        }
    }

    public string GenerateSalt()
    {
        try
        {
            var buffer = new byte[16];
            RandomNumberGenerator.Fill(buffer);

            return Convert.ToBase64String(buffer);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Error while generating salt. (PlayerService)");
            throw;
        }
    }
}