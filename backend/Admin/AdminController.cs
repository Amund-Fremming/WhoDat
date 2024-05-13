using PlayerEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GalleryEntity;
using Dto;

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
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpGet("players")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        try
        {
            IEnumerable<PlayerDto> players = await _playerService.GetAllPlayers();

            return Ok(players);
        }
        catch (Exception e)
        {
            return HandleException(e);
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
