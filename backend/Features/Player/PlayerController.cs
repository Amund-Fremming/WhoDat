using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerRepository playerRepository) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    [HttpPut("players/update-username")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayerUsername([FromBody] string newUsername)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string encodedNewUsername = EncodeForJsAndHtml(newUsername);

            var result = await _playerRepository.UpdateUsername(playerId, encodedNewUsername);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdatePlayerUsername)");
            return StatusCode(500);
        }
    }

    [HttpPut("players/update-password")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayerPassword([FromBody] string newPassword)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string encodedNewPassword = EncodeForJsAndHtml(newPassword);

            var result = await _playerRepository.UpdatePassword(playerId, encodedNewPassword);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdatePlayerPassword)");
            return StatusCode(500);
        }
    }

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    [NonAction]
    private static string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}