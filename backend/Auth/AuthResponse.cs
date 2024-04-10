namespace Auth;

public class AuthResponse
{
    public string PlayerID { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

    public AuthResponse(string playerId, string username, string token)
    {
        PlayerID = playerId;
        Username = username;
        Token = token;
    }
}
