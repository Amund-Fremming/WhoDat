using Backend.Features.Player;

namespace Backend.Features.Auth;

public record AuthResponse(int PlayerId, string Username, string Token)
{
    public static AuthResponse Convert(PlayerEntity player, string token) => new(player.ID, player.Username, token);
}