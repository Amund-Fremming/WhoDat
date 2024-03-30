namespace PlayerEntity;
using System.ComponentModel.DataAnnotations;

public class Player
{
    public int PlayerID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}
