namespace PlayerEntity;

public interface IPlayerService
{
    public Task<Player> CreatePlayer(Player player);
    public Task<bool> DeletePlayer(int playerId);
    public Task<bool> UpdateUsername(int playerId, string newUsername);
}
