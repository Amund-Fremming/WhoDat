namespace PlayerEntity;

public interface IPlayerService
{
    /* Basic CRUD */
    public Task<Player> GetPlayerById(int playerId);
    public Task<string> CreatePlayer(Player player);
    public Task<bool> DeletePlayer(int playerId);

    /* Other */
    public Task<bool> DoesUsernameExist(string username);
    public Task<bool> UpdateUsername(string playerId, string newUsername);
}
