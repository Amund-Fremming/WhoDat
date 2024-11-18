namespace Backend.Features.Player;

public interface IPlayerRepository
{
    /// <summary>
    /// Get a Player corresponding to the given id.
    /// </summary>
    /// <param name="playerId">The id for the Player.</param>
    /// <returns>The Player asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Player does not exist.</exception>
    Task<PlayerEntity> GetPlayerById(int playerId);

    /// <summary>
    /// Stores a new Player to the database.
    /// </summary>
    /// <param name="player">The Player to be stored.</param>
    Task<int> CreatePlayer(PlayerEntity player);

    /// <summary>
    /// Deletes a Player from the database.
    /// </summary>
    /// <param name="player">The Player to be deleted</param>
    Task DeletePlayer(PlayerEntity player);

    /// <summary>
    /// Fetches a Player by giving their username.
    /// </summary>
    /// <param name="username">The Players username.</param>
    /// <returns>The player asked for.</returns>
    Task<PlayerEntity> GetPlayerByUsername(string username);

    /// <summary>
    /// Updates the username for a given Player.
    /// </summary>
    /// <param name="player">The player to update its username.</param>
    /// <param name="newUsername">The new username.</param>
    Task UpdateUsername(PlayerEntity player, string newUsername);

    /// <summary>
    /// Updates the password and salt for a player.
    /// </summary>
    /// <param name="player">The player to update on.</param>
    /// <param name="newPassword">The new hashed password.</param>
    /// <param name="newSalt">The new generated salt.</param>
    Task UpdatePassword(PlayerEntity player, string newPassword, string newSalt);

    /// <summary>
    /// Fetches all Players.
    /// </summary>
    /// <returns>An Enumerable of Players.</returns>
    Task<IEnumerable<PlayerDto>> GetAllPlayers();

    /// <summary>
    /// Tries to fint a username in the database, and throws if it exists.
    /// </summary>
    /// <param name="username">The username to seach for.</param>
    /// <exception cref="ArgumentException">Throws if the username was found.</exception>
    Task DoesUsernameExist(string username);
}