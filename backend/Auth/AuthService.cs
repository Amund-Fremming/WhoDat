using PlayerEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using GalleryEntity;
using Enum;
using Data;

namespace Auth;

public class AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger,
        IPasswordHasher<Player> passwordHasher, PlayerRepository playerRepository,
        IGalleryService galleryService, IPlayerService playerService) : IAuthService
{
    public readonly AppDbContext _context = context;
    public readonly IConfiguration _configuration = configuration;
    public readonly ILogger<AuthService> _logger = logger;
    public readonly IPasswordHasher<Player> _passwordHasher = passwordHasher;
    public readonly IPlayerService _playerService = playerService;
    public readonly PlayerRepository _playerRepository = playerRepository;
    public readonly IGalleryService _galleryService = galleryService;

    public string GenerateToken(Player player)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var configurationKey = _configuration["Jwt:Key"];

            if (configurationKey == null) throw new KeyNotFoundException("Jwt key not present in appsettings. (AuthService)");

            var key = Encoding.ASCII.GetBytes(configurationKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, player.PlayerID.ToString()),
                    new Claim(ClaimTypes.Role, player.Role.ToString()),
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
            // ADD HANDLING
            _logger.LogError(e, "Error while generating token. (AuthService)");
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
            // ADD HANDLING
            _logger.LogError(e, "Error while generating salt. (AuthService)");
            throw;
        }
    }

    public async Task<bool> ValidatePasswordWithSalt(LoginRequest request, string password)
    {
        try
        {
            Player player = await _playerRepository.GetPlayerByUsername(request.Username);

            var saltedPassword = request.Password + player.PasswordSalt;
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, saltedPassword);

            return (result == PasswordVerificationResult.Success);

        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, "Error while validating password with salt. (AuthService)");
            throw;
        }
    }

    public async Task<Player> RegisterNewPlayer(RegistrationRequest request)
    {
        try
        {
            string salt = GenerateSalt();
            string saltedPassword = request.Password + salt;
            string hashedPassword = _passwordHasher.HashPassword(null!, saltedPassword);

            Player player = new Player(request.Username, hashedPassword, salt, Role.USER);
            Player newPlayer = await _playerService.CreatePlayer(player);
            await _galleryService.CreateGallery(newPlayer.PlayerID);

            return newPlayer;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, "Error while validating password with salt. (AuthService)");
            throw;
        }
    }
}
