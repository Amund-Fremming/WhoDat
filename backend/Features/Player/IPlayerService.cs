using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

public interface IPlayerService
{
    /// <summary>
    /// Creates a new player.
    /// </summary>
    /// <param name="player">The new player to be created.</param>
    /// <returns>The newly created players id.</returns>
    /// <exception cref="ArgumentException">Throws if the username exist.</exception>
    public Task<Result> CreatePlayer(PlayerEntity player);

    /// <summary>
    /// Deletes a player, only admin or this player can delete.
    /// </summary>
    /// <param name="playerId">The player to be deleted.</param>
    /// <exception cref="KeyNotFoundException">Throws if the player does not exist.</exception>
    public Task<Result> DeletePlayer(int playerId);

    /// <summary>
    /// Updates a players username, also checks that the new username does not exist.
    /// </summary>
    /// <param name="playerId">The player updating its username.</param>
    /// <param name="newUsername">The new username to be set.</param>
    /// <exception cref="KeyNotFoundException">Throws if the player does not exist.</exception>
    /// <exception cref="ArgumentException">Throws if the username exist.</exception>
    public Task<Result> UpdateUsername(int playerId, string newUsername);

    /// <summary>
    /// updates a players password.
    /// </summary>
    /// <param name="playerId">The player to update its password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <exception cref="KeyNotFoundException">Throws if the player does not exist.</exception>
    public Task<Result> UpdatePassword(int playerId, string newPassword);

    /// <summary>
    /// Fetches all players that exist.
    /// </summary>
    /// <returns>A collection of player dtos containing only relevant playerdata, no passwords or salt.</returns>
    public Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers();

    /// <summary>
    /// Generates salt for seeding passwords.
    /// </summary>
    /// <returns>Generated salt</returns>
    public string GenerateSalt();
}