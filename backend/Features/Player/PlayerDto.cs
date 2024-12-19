namespace Backend.Features.Player;

public class PlayerDto(int PlayerID, string Username, string? Password, string? ImageUrl)
{
    public int PlayerID { get; set; } = PlayerID;
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
    public string ImageUrl { get; set; } = ImageUrl;
}