using Dto;

namespace PlayerEntity;

public interface IPlayerRepository
{
    Task<Player> GetPlayerById(int playerId);

    Task<int> CreatePlayer(Player player);

    Task DeletePlayer(Player player);

    Task<Player> GetPlayerByUsername(string username);

    Task UpdateUsername(Player player, string newUsername);

    Task UpdatePassword(Player player, string newPassword, string newSalt);

    Task<IEnumerable<PlayerDto>> GetAllPlayers();

    Task DoesUsernameExist(string username);
}
