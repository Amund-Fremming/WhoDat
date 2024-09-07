namespace PlayerEntity;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerService playerService, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerService _playerService = playerService;
    public readonly ICardService _cardService = cardService;

    [HttpPut("players/update-username")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayerUsername([FromBody] string newUsername)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string encodedNewUsername = EncodeForJsAndHtml(newUsername);

            await _playerService.UpdateUsername(playerId, encodedNewUsername);
            return Ok("Username Updated!");
        }
        catch (Exception e)
        {
            return HandleException(e);
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

            await _playerService.UpdatePassword(playerId, encodedNewPassword);
            return Ok("Password Updated!");
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpGet("cards/allcards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();

            IEnumerable<Card> cards = await _cardService.GetAllCards(playerId);
            return Ok(cards);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpPost("cards/add")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> AddCard([FromForm] CardInputDto cardDto)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            int cardId = await _cardService.CreateCard(playerId, cardDto);

            return Ok($"Card Created {cardId}");
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    private ActionResult HandleException(Exception exception)
    {
        switch (exception)
        {
            case InvalidOperationException _:
                return BadRequest(exception.Message);
            case KeyNotFoundException _:
                return NotFound(exception.Message);
            case UnauthorizedAccessException _:
                return Unauthorized(exception.Message);
            case ArgumentException _:
                return Conflict(exception.Message);
            default:
                return StatusCode(500, exception.Message);
        }
    }

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    [NonAction]
    private string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}
