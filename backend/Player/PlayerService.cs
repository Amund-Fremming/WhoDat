namespace PlayerEntity;

public class PlayerService(PlayerRepository playerRepository) : IPlayerService
{
    public readonly PlayerRepository _playerRepository = playerRepository;

    public Task<Player?> GetPlayerById(int playerId)
    {

    }

    public Task<string> CreatePlayer(Player player)
    {

    }

    public Task<bool> DeletePlayer(int playerId)

    {

    }

    public Task<bool> DoesUsernameExist(string username)
    {

    }

    public Task<bool> UpdateUsername(string playerId, string newUsername)
    {

    }
}
