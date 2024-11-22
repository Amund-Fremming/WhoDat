using Backend.Features.Player;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Admin;

[ApiController]
[Route("api/[controller]")]
public class AdminController(ILogger<AdminController> logger, IPlayerRepository playerRepository) : ControllerBase
{
    public readonly IPlayerRepository _playerRepository = playerRepository;
    public readonly ILogger<AdminController> _logger = logger;

    [HttpDelete("players/delete/{playerId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> DeletePlayer(int playerId)
    {
        try
        {
            var result = await _playerRepository.DeletePlayer(playerId);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeletePlayer)");
            return StatusCode(500);
        }
    }

    [HttpGet("players")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> GetAllPlayers()
    {
        try
        {
            var result = await _playerRepository.GetAllPlayers();
            return result.Resolve(
                suc => Ok(suc.Data),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetAllPlayers)");
            return StatusCode(500);
        }
    }
}