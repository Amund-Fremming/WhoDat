namespace PlayerEntity;

public interface IPlayerService
{
    /* Basic CRUD */
    public Task<int> CreatePlayer(Player player);
    public Task<bool> DeletePlayer(int playerId);

    /* Other */
    public Task<bool> UpdateUsername(int playerId, string newUsername);
}
