using Backend.Features.Player;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

[ApiController]
[Route("api/[controller]")]
public class CardController(ILogger<PlayerController> logger, IPlayerRepository playerRepository, ICardRepository cardRepository, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerRepository _playerRepository = playerRepository;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly ICardService _cardService = cardService;

    [HttpGet("getall")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<CardEntity>>> GetAll()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();

            var result = await _cardRepository.GetAllCards(playerId);
            return result.Resolve(
                suc => Ok(suc.Data),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetAll)");
            return StatusCode(500);
        }
    }

    [HttpPost("add")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> Add()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();

            // Read the content type from the request
            string contentType = Request.ContentType ?? "application/octet-stream";

            // Read the image data from the request body
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            // Retrieve the name from the headers
            string name = Request.Headers["X-Card-Name"].ToString();

            // Create an IFormFile instance
            var formFile = new FormFile(new MemoryStream(imageData), 0, imageData.Length, "Image", "image.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            var cardDto = new CreateCardDto
            {
                Name = name,
                Image = formFile
            };

            var result = await _cardService.CreateCard(playerId, cardDto);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(Add)");
            return StatusCode(500);
        }
    }

    [HttpDelete("delete/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> Delete(int cardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            var result = await _cardService.DeleteCard(playerId, cardId);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(Delete)");
            return StatusCode(500);
        }
    }

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
}