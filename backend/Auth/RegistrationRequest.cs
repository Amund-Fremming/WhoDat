namespace Auth;
using System.ComponentModel.DataAnnotations;

public class RegistrationRequest
{
    [Required]
    [StringLength(15, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Username { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
    public string Password { get; set; }

    public RegistrationRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
