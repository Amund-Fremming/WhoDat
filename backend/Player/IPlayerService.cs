namespace PlayerEntity;

public interface IPlayerService
{
    /* Basic CRUD */
    public Task<Player> GetPlayerById(int playerId);
    public Task<int> CreatePlayer(Player player);
    public Task DeletePlayer(int playerId);
    public Task UpdatePlayer(Player player);

    /* Other */
    public Task<bool> DoesUsernameExist(string username);
}
