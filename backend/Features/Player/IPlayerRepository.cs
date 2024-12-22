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
    /// Updates a player
    /// </summary>
    /// <param name="playerDto">Player to update</param>
    /// <returns>Updated player</returns>
    Task<Result<PlayerDto>> Update(PlayerDto playerDto);

    /// <summary>
    /// Fetches all Players.
    /// </summary>
    /// <returns>An Enumerable of Players.</returns>
    Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers();

    /// <summary>
    /// Tries to find a username in the database, and throws if it exists.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <exception cref="ArgumentException">Throws if the username was found.</exception>
    Task<Result> UsernameExist(string username);
}