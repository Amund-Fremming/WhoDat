using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Auth;

public class AuthService(AppDbContext context, IConfiguration configuration, ILogger<IAuthService> logger,
        IPasswordHasher<PlayerEntity> passwordHasher, IPlayerRepository playerRepository) : IAuthService
{
    public readonly AppDbContext _context = context;
    public readonly IConfiguration _configuration = configuration;
    public readonly ILogger<IAuthService> _logger = logger;
    public readonly IPasswordHasher<PlayerEntity> _passwordHasher = passwordHasher;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public Result<string> GenerateToken(PlayerEntity player)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var configurationKey = _configuration["Jwt:Key"] ?? throw new KeyNotFoundException("Jwt key not present in appsettings. (AuthService)");
            var key = Encoding.ASCII.GetBytes(configurationKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, player.PlayerID.ToString()),
                    new Claim(ClaimTypes.Role, player.PlayerRole.ToString()),
                    new Claim(ClaimTypes.Name, player.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(1),       // TODO - JUSTER DENNE!!!
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
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

    public string GenerateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator.Fill(buffer);

        return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// Does not return anything, but throws if password is not valid.
    /// </summary>
    /// <param name="request">Login request</param>
    /// <exception cref="UnauthorizedAccessException">If password is not valid</exception>
    public async Task ValidatePasswordWithSalt(LoginRequest request)
    {
        try
        {
            var result = await _playerRepository.GetPlayerByUsername(request.Username);
            if (result.IsError)
                throw new UnauthorizedAccessException("Unauthorized.");

            var player = result.Data;

            var saltedPassword = request.Password + player.PasswordSalt;
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, saltedPassword);

            if (verificationResult != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Unauthorized.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while validating password with salt. (AuthService)");
            throw new UnauthorizedAccessException("Unauthorized.");
        }
    }

    public async Task<Result<PlayerEntity>> RegisterNewPlayer(RegistrationRequest request)
    {
        try
        {
            string salt = GenerateSalt();
            string saltedPassword = request.Password + salt;
            string hashedPassword = _passwordHasher.HashPassword(null!, saltedPassword);

            PlayerEntity player = new(request.Username, hashedPassword, salt, PlayerRole.USER);
            var result = await _playerRepository.CreatePlayer(player);
            if (result.IsError)
                return result.ToResult<PlayerEntity>();

            return player;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(RegisterNewPlayer)");
            return new Error(e, "Registration failed.");
        }
    }
}