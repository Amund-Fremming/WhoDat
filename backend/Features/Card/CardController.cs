using Backend.Features.Player;

namespace Backend.Features.Card;

[ApiController]
[Route("api/[controller]")]
public class CardController(ILogger<PlayerController> logger, IPlayerService playerService, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerService _playerService = playerService;
    public readonly ICardService _cardService = cardService;

    [HttpGet("getall")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<CardEntity>>> GetAll()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();

            IEnumerable<CardEntity> cards = await _cardService.GetAllCards(playerId);
            return Ok(cards);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpPost("test-upload")]
    public async Task<IActionResult> TestUpload()
    {
        using (var memoryStream = new MemoryStream())
        {
            await Request.Body.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            var tempFilePath = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "test_upload_image.jpg");
            await System.IO.File.WriteAllBytesAsync(tempFilePath, imageData);

            _logger.LogInformation($"Image saved to: {tempFilePath}", tempFilePath);
        }

        return Ok("Upload successful");
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

            int cardId = await _cardService.CreateCard(playerId, cardDto);

            return Ok($"Card Created {cardId}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while adding a card");
            return BadRequest(e.Message);
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