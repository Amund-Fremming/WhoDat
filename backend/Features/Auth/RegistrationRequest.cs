namespace Backend.Features.Auth;

public class RegistrationRequest(string username, string password)
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Username { get; set; } = username;

    [Required]
    public string Password { get; set; } = password;
}