using Microsoft.AspNetCore.Mvc;
using PlayerEntity;

namespace Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService,
        IPlayerService playerService, PlayerRepository playerRepository) : ControllerBase
{
    public readonly ILogger<AuthController> _logger = logger;
    public readonly IAuthService _authService = authService;
    public readonly IPlayerService _playerService = playerService;
    public readonly PlayerRepository _playerRepository = playerRepository;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            bool validPassword = await _authService.ValidatePasswordWithSalt(request, request.Password);

            if (!validPassword)
                return Unauthorized("Invalid credentials");

            Player player = await _playerRepository.GetPlayerByUsername(request.Username);
            string token = _authService.GenerateToken(player);

            return Ok(new AuthResponse(player.PlayerID, player.Username, token));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewPlayer([FromBody] RegistrationRequest request)
    {
        try
        {
            bool usernameExist = await _playerRepository.DoesUsernameExist(request.Username);

            if (usernameExist)
                return Conflict("Username already exists");

            Player? registeredPlayer = await _authService.RegisterNewPlayer(request);
            string token = _authService.GenerateToken(registeredPlayer);

            return Ok(new AuthResponse(registeredPlayer.PlayerID, registeredPlayer.Username, token));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
