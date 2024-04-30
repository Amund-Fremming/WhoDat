using PlayerEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GalleryEntity;

namespace Admin;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IPlayerService playerService, IGalleryService galleryService) : ControllerBase
{
    public readonly IPlayerService _playerService = playerService;
    public readonly IGalleryService _galleryService = galleryService;

    [HttpDelete("players/delete/{playerId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> DeletePlayer(int playerId)
    {
        try
        {
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

    [HttpDelete("galleries/{galleryId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> DeleteGallery(int galleryId, int playerId)
    {
        try
        {
            await _galleryService.DeleteGallery(playerId, galleryId);

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
