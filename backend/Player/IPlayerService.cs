namespace PlayerEntity;

public interface IPlayerService
{
    /* Basic CRUD */
    public Task<Player> GetPlayerById(int playerId);
    public Task<int> CreatePlayer(Player player);
    public Task DeletePlayer(int playerId);

    /* Other */
    public Task<bool> DoesUsernameExist(string username);
    public Task<bool> UpdateUsername(string newUsername);
    public Task<bool> ChangePassword(string newPassword);
}
