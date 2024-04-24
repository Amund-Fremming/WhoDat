namespace Auth;
using PlayerEntity;

public interface IAuthService
{
    string GenerateToken(Player player);
    string GenerateSalt();
    Task ValidatePasswordWithSalt(LoginRequest loginRequest, string password);
    Task<Player> RegisterNewPlayer(RegistrationRequest request);
}

