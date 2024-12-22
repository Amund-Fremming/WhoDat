using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerService playerService) : ControllerBase
{
    private readonly ILogger<PlayerController> _logger = logger;
    private readonly IPlayerService _playerService = playerService;

    [HttpPut("update")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> Update([FromBody] PlayerDto playerDto)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string encodedNewUsername = EncodeForJsAndHtml(playerDto.Username);
            playerDto.PlayerID = playerId;
            playerDto.Username = encodedNewUsername;

            var result = await _playerService.Update(playerDto);
            return result.Resolve(
                suc => Ok(suc.Data),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdatePlayerUsername)");
            return StatusCode(500);
        }
    }

    [HttpPut("update-image")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdateImage()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            FormFile formFile = null;
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);
                formFile = new FormFile(memoryStream, 0, memoryStream.Length, "Image", "image.jpg");
            }

            if (formFile == null)
            {
                return Ok("There was no image to upload.");
            }

            var result = await _playerService.UpdateImage(playerId, formFile);
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