using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Auth;

public class AuthService(AppDbContext context, IConfiguration configuration, ILogger<IAuthService> logger,
        IPasswordHasher<PlayerEntity> passwordHasher, IPlayerRepository playerRepository,
        IPlayerService playerService) : IAuthService
{
    public readonly AppDbContext _context = context;
    public readonly IConfiguration _configuration = configuration;
    public readonly ILogger<IAuthService> _logger = logger;
    public readonly IPasswordHasher<PlayerEntity> _passwordHasher = passwordHasher;
    public readonly IPlayerService _playerService = playerService;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public string GenerateToken(PlayerEntity player)
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
                    new Claim(ClaimTypes.Role, player.UserRole.ToString()),
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
            _logger.LogError(e.Message, "Error while generating token. (AuthService)");
            throw;
        }
    }

    public string GenerateSalt()
    {
        try
        {
            var buffer = new byte[16];
            RandomNumberGenerator.Fill(buffer);

            return Convert.ToBase64String(buffer);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Error while generating salt. (AuthService)");
            throw;
        }
    }

    public async Task ValidatePasswordWithSalt(LoginRequest request)
    {
        try
        {
            PlayerEntity player = await _playerRepository.GetPlayerByUsername(request.Username);

            var saltedPassword = request.Password + player.PasswordSalt;
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, saltedPassword);

            if (result != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Credentials not valid!");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Error while validating password with salt. (AuthService)");
            throw new UnauthorizedAccessException("Credentials not valid!");
        }
    }

    public async Task<PlayerEntity> RegisterNewPlayer(RegistrationRequest request)
    {
        try
        {
            string salt = GenerateSalt();
            string saltedPassword = request.Password + salt;
            string hashedPassword = _passwordHasher.HashPassword(null!, saltedPassword);

            PlayerEntity player = new(request.Username, hashedPassword, salt, PlayerRole.USER);
            await _playerService.CreatePlayer(player);

            return player;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Error while validating password with salt. (AuthService)");
            throw;
        }
    }
}