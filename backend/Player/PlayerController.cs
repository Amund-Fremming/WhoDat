using Microsoft.AspNetCore.Mvc;
using GalleryEntity;
using CardEntity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PlayerEntity;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerService playerService, IGalleryService galleryService, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerService _playerService = playerService;
    public readonly IGalleryService _galleryService = galleryService;
    public readonly ICardService _cardService = cardService;

    [HttpDelete("players/delete")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> DeletePlayer()
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _playerService.DeletePlayer(playerId);

            return Ok("Player Deleted!");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [HttpPut("players/update-username")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayerUsername([FromBody] string newUsername)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _playerService.UpdateUsername(playerId, newUsername);

            return Ok("Username Updated!");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [HttpGet("galleries/{galleryId}/cards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<Card>>> GetPlayerGalleryCards(int galleryId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            IEnumerable<Card> cards = await _cardService.GetAllCards(playerId, galleryId);

            return Ok(cards);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [HttpPost("galleries/{galleryId}/cards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> AddCardToGallery(int galleryId, [FromBody] Card card)
    {
        try
        {
            card.GalleryID = galleryId;
            int playerId = ParsePlayerIdClaim();
            int cardId = await _cardService.CreateCard(playerId, card);

            return Ok($"Card Created {cardId}");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [HttpPut("galleries/{galleryId}/cards/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdateCardInGallery(int galleryId, int cardId, [FromBody] Card updatedCard)
    {
        try
        {
            updatedCard.GalleryID = galleryId;
            updatedCard.CardID = cardId;
            int playerId = ParsePlayerIdClaim();
            await _cardService.UpdateCard(playerId, updatedCard);

            return Ok("Card Updated!");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [HttpDelete("galleries/{galleryId}/cards/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> DeleteCardInGallery(int galleryId, int cardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _cardService.DeleteCard(playerId, cardId);

            return Ok("Card Deleted!");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
}
