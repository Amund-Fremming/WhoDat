using System.ComponentModel.DataAnnotations;

namespace PlayerEntity;

public class Player
{
    [Key]
    public int PlayerID { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }

    public Player() { }
}
