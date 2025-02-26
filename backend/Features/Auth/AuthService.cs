using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Auth;

public class AuthService(IConfiguration configuration, ILogger<IAuthService> logger,
        IPasswordHasher<PlayerEntity> passwordHasher, IPlayerRepository playerRepository) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<IAuthService> _logger = logger;
    private readonly IPasswordHasher<PlayerEntity> _passwordHasher = passwordHasher;
    private readonly IPlayerRepository _playerRepository = playerRepository;

    public Result<string> GenerateToken(PlayerEntity player)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var configurationKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new KeyNotFoundException("(AuthService) Jwt key not present.");
            var key = Encoding.ASCII.GetBytes(configurationKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, player.ID.ToString()),
                    new Claim(ClaimTypes.Role, player.PlayerRole.ToString()),
                    new Claim(ClaimTypes.Name, player.Username),
                ]),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new KeyNotFoundException("(AuthService) Jwt Issuer not present."),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new KeyNotFoundException("(AuthService) Jwt Audience not present.")
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GenerateToken)");
            return new Error(e, "Authentication failed.");
        }
    }

    private static string GenerateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator.Fill(buffer);

        return Convert.ToBase64String(buffer);
    }

    private async Task<Result> ValidatePasswordWithSalt(LoginRequest request)
    {
        try
        {
            var result = await _playerRepository.GetPlayerByUsername(request.Username);
            if (result.IsError)
                return new Error(new UnauthorizedAccessException("Password or username is wrong."), "Password or username is wrong.");

            var player = result.Data;
            var saltedPassword = request.Password + player.PasswordSalt;
            var verificationResult = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, saltedPassword);

            if (verificationResult != PasswordVerificationResult.Success)
                return new Error(new UnauthorizedAccessException("Password or username is wrong."), "Password or username is wrong.");

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while validating password with salt. (AuthService)");
            return new Error(new UnauthorizedAccessException("Password or username is wrong."), "Password or username is wrong.");
        }
    }

    public async Task<Result<PlayerEntity>> RegisterNewPlayer(RegistrationRequest request)
    {
        try
        {
            var salt = GenerateSalt();
            var saltedPassword = request.Password + salt;
            var hashedPassword = _passwordHasher.HashPassword(null!, saltedPassword);

            PlayerEntity player = new(request.Username, hashedPassword, salt, PlayerRole.USER, "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg");
            var result = await _playerRepository.Create(player);
            
            return result.IsError ? result.ToResult<int, PlayerEntity>() : player;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(RegisterNewPlayer)");
            return new Error(e, "Registration failed.");
        }
    }
}