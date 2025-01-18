namespace Backend.Features.Auth;

public class LoginRequest(string Username, string Password)
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Username { get; set; } = Username;

    [Required]
    [StringLength(15, MinimumLength = 5)]
    public string Password { get; set; } = Password;
}