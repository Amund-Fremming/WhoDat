namespace PlayerEntity;

public class PlayerService(ILogger<PlayerService> logger, PlayerRepository playerRepository) : IPlayerService
{
    public readonly PlayerRepository _playerRepository = playerRepository;
    public readonly ILogger<PlayerService> _logger = logger;

    public async Task<int> CreatePlayer(Player player)
    {
        try
        {
            await _playerRepository.GetPlayerById(player.PlayerID);

            return await _playerRepository.CreatePlayer(player);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while creating Player with id {player.PlayerID}. (PlayerService)");
            throw;
        }
    }

    public async Task<bool> DeletePlayer(int playerId)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);

            return await _playerRepository.DeletePlayer(player);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while deleting Player with id {playerId}. (PlayerService)");
            throw;
        }
    }

    public async Task<bool> UpdateUsername(int playerId, string newUsername)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);

            return await _playerRepository.UpdateUsername(player, newUsername);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while updating username for Player with id {playerId}. (PlayerService)");
            throw;
        }
    }
}
