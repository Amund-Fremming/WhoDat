namespace Backend.Features.Auth;

public class AuthResponse(int playerId, string username, string token)
{
    public int PlayerID { get; set; } = playerId;
    public string Username { get; set; } = username;
    public string Token { get; set; } = token;
}