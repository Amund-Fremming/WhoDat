namespace Auth;
using PlayerEntity;

public interface IAuthService
{
    string GenerateToken(Player player);
    string GenerateSalt();
}

