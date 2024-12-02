using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerRepository playerRepository) : ControllerBase
{
    private readonly ILogger<PlayerController> _logger = logger;
    private readonly IPlayerRepository _playerRepository = playerRepository;

    [HttpPut("players/update")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayer([FromBody] PlayerDto playerDto)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string encodedNewUsername = EncodeForJsAndHtml(playerDto.Username);
            playerDto.Username = encodedNewUsername;

            var result = await _playerRepository.Update(playerDto);
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

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    [NonAction]
    private static string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}