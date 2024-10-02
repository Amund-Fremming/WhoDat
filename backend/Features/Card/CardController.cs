namespace CardEntity;

[ApiController]
[Route("api/[controller]")]
public class CardController(ILogger<PlayerController> logger, IPlayerService playerService, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerService _playerService = playerService;
    public readonly ICardService _cardService = cardService;

    [HttpGet("getall")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<Card>>> GetAll()
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

    [HttpPost("add")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> Add([FromForm] CardInputDto cardDto)
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

    [HttpDelete("delete/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> Delete(int cardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _cardService.DeleteCard(playerId, cardId);

            return Ok($"Card Deleted{cardId}");
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
}
