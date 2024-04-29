namespace PlayerEntity;

public interface IPlayerService
{
    public Task<Player> CreatePlayer(Player player);
    public Task DeletePlayer(int playerId);
    public Task UpdateUsername(int playerId, string newUsername);
    public Task UpdatePassword(int playerId, string newUsername);
}
