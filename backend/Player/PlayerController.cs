using Microsoft.AspNetCore.Mvc;
using GalleryEntity;
using CardEntity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PlayerEntity;

[ApiController]
[Route("api/")]
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            // TODO
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
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("players/update-username")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayerUsername([FromBody] string newUsername)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        try
        {
            // TODO
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
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("galleries/{galleryId}/cards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<Card>>> GetPlayerGalleryCards(int galleryId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        try
        {
            // TODO
            return Ok(Enumerable.Empty<string>());
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("galleries/{galleryId}/cards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<Card>> AddCardToGallery(int galleryId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        try
        {
            // TODO
            return Ok(new Card());
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("galleries/{galleryId}/cards/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<Card>> UpdateCardInGallery(int galleryId, int cardId, Card updatedCard)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            // TODO
            return Ok(new Card());
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("galleries/{galleryId}/cards/{cardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<IEnumerable<Card>>> DeleteCardInGallery(int galleryId, int cardId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        try
        {
            // TODO
            return Ok(Enumerable.Empty<string>());
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
