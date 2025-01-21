using Backend.Features.Player;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Auth;

public interface IAuthService
{
    Result<string> GenerateToken(PlayerEntity player);

    Task<Result<PlayerEntity>> RegisterNewPlayer(RegistrationRequest request);
}