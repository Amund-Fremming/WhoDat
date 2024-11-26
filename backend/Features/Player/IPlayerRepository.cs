using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

public interface IPlayerRepository : IRepository<PlayerEntity>
{
    /// <summary>
    /// Fetches a Player by giving their username.
    /// </summary>
    /// <param name="username">The Players username.</param>
    /// <returns>The player asked for.</returns>
    Task<Result<PlayerEntity>> GetPlayerByUsername(string username);

    /// <summary>
    /// Updates the username for a given Player.
    /// </summary>
    /// <param name="playerId">The player to update its username.</param>
    /// <param name="newUsername">The new username.</param>
    Task<Result> UpdateUsername(int playerId, string newUsername);

    /// <summary>
    /// Updates the password and salt for a player.
    /// </summary>
    /// <param name="playerId">The player to update on.</param>
    /// <param name="newPassword">The new hashed password.</param>
    Task<Result> UpdatePassword(int playerId, string newPassword);

    /// <summary>
    /// Fetches all Players.
    /// </summary>
    /// <returns>An Enumerable of Players.</returns>
    Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers();

    /// <summary>
    /// Tries to fint a username in the database, and throws if it exists.
    /// </summary>
    /// <param name="username">The username to seach for.</param>
    /// <exception cref="ArgumentException">Throws if the username was found.</exception>
    Task<Result> DoesUsernameExist(string username);
}