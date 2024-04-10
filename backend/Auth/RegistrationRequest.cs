namespace Auth;
using System.ComponentModel.DataAnnotations;

public class RegistrationRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public RegistrationRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
