namespace Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService,
        IPlayerService playerService, IPlayerRepository playerRepository) : ControllerBase
{
    public readonly ILogger<AuthController> _logger = logger;
    public readonly IAuthService _authService = authService;
    public readonly IPlayerService _playerService = playerService;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            await _authService.ValidatePasswordWithSalt(request);

            PlayerEntity.Player player = await _playerRepository.GetPlayerByUsername(request.Username);
            string token = _authService.GenerateToken(player);

            return Ok(new AuthResponse(player.PlayerID, player.Username, token));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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
            await _playerRepository.DoesUsernameExist(request.Username);

            PlayerEntity.Player? registeredPlayer = await _authService.RegisterNewPlayer(request);
            string token = _authService.GenerateToken(registeredPlayer);

            return Ok(new AuthResponse(registeredPlayer.PlayerID, registeredPlayer.Username, token));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}