using System;

namespace PlayerEntity;

public class PlayerService(PlayerRepository playerRepository) : IPlayerService
{
    public readonly PlayerRepository _playerRepository = playerRepository;

    public Task<Player> GetPlayerById(int playerId)
    {

    }

    public Task<int> CreatePlayer(Player player)
    {

    }

    public Task DeletePlayer(int playerId)
    {

    }

    public Task<bool> DoesUsernameExist(string username)
    {

    }

    public Task<bool> UpdateUsername(string newUsername)
    {

    }

    public Task<bool> ChangePassword(string newPassword)
    {

    }
}
