namespace PlayerEntity;

public class PlayerService(ILogger<PlayerService> logger, PlayerRepository playerRepository) : IPlayerService
{
    public readonly PlayerRepository _playerRepository = playerRepository;
    public readonly ILogger<PlayerService> _logger = logger;

    public async Task<Player> CreatePlayer(Player player)
    {
        try
        {
            await _playerRepository.CreatePlayer(player);
            return player;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Player with id {player.PlayerID}. (PlayerService)");
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
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Player with id {playerId}. (PlayerService)");
            throw;
        }
    }

    public async Task UpdateUsername(int playerId, string newUsername)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerById(playerId);

            await _playerRepository.UpdateUsername(player, newUsername);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating username for Player with id {playerId}. (PlayerService)");
            throw;
        }
    }
}
