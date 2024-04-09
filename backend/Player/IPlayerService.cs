namespace PlayerEntity;

public interface IPlayerService
{
    /* Basic CRUD */
    public Task<string> CreatePlayer(Player player);
    public Task<bool> DeletePlayer(string playerId);

    /* Other */
    public Task<bool> UpdateUsername(string playerId, string newUsername);
}
