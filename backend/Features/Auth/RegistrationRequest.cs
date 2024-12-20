namespace Backend.Features.Auth;

public class RegistrationRequest(string Username, string Password)
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Username { get; set; } = Username;

    [Required]
    [StringLength(15, MinimumLength = 5)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,}$", ErrorMessage = "Password must be at least 5 characters long and contain at least one lowercase letter, one uppercase letter, and one digit.")]
    public string Password { get; set; } = Password;
}