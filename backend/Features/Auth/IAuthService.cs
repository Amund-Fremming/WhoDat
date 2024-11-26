using Backend.Features.Player;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Auth;

public interface IAuthService
{
    /// <summary>
    /// Generates a token used for authorization and authentication.
    /// </summary>
    /// <param name="player">The player to generate a token for.</param>
    /// <returns>The generated token as a string.</returns>
    Result<string> GenerateToken(PlayerEntity player);

    /// <summary>
    /// Generates the salt used to be stored with a players password.
    /// </summary>
    /// <returns>The generated salt as a string.</returns>
    string GenerateSalt();

    /// <summary>
    /// Validates that the password a player provides is correct.
    /// This method does not return anything, but throws it the password is invalid.
    /// </summary>
    /// <param name="loginRequest">Object containing all data needed for a login.</param>
    Task ValidatePasswordWithSalt(LoginRequest loginRequest);

    /// <summary>
    /// Registers a new player by using the player service.
    /// </summary>
    /// <param name="request">Object containing all data for regirering a user.</param>
    /// <returns>The Player created.</returns>
    Task<Result<PlayerEntity>> RegisterNewPlayer(RegistrationRequest request);
}