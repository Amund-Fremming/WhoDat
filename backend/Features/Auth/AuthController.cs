using Backend.Features.Player;

namespace Backend.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService, IPlayerRepository playerRepository) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthService _authService = authService;
    private readonly IPlayerRepository _playerRepository = playerRepository;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            await _authService.ValidatePasswordWithSalt(request);

            var result = await _playerRepository.GetPlayerByUsername(request.Username);
            if (result.IsError)
                return BadRequest(result.Message);

            var player = result.Data;
            var tokenResult = _authService.GenerateToken(player);
            if (tokenResult.IsError)
                return BadRequest(result.Message);

            var token = tokenResult.Data;
            return Ok(AuthResponse.Convert(player, token));
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError(e, "(Login) - Unauthorized, problem lies under ValidatePasswordWithSalt.");
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(Login)");
            return StatusCode(500);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewPlayer([FromBody] RegistrationRequest request)
    {
        try
        {
            var usernameResult = await _playerRepository.UsernameExist(request.Username);
            if (usernameResult.IsError)
                return BadRequest(usernameResult.Message);

            var playerResult = await _authService.RegisterNewPlayer(request);
            if (playerResult.IsError)
                return BadRequest(playerResult.Message);

            var tokenResult = _authService.GenerateToken(playerResult.Data);
            if (tokenResult.IsError)
                return BadRequest(tokenResult.Message);

            return Ok(AuthResponse.Convert(playerResult.Data, tokenResult.Data));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(RegisterNewPlayer)");
            return StatusCode(500, e.Message);
        }
    }
}