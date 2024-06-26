namespace Auth;

public class AuthResponse
{
    public int PlayerID { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

    public AuthResponse(int playerId, string username, string token)
    {
        PlayerID = playerId;
        Username = username;
        Token = token;
    }
}
