using Microsoft.AspNetCore.Mvc;
using BoardEntity;
using BoardCardEntity;
using MessageEntity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GameEntity;

[ApiController]
[Route("api/[controller]")]
public class GameController(ILogger<GameController> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService) : ControllerBase
{
    public readonly ILogger<GameController> _logger = logger;
    public readonly IGameService _gameService = gameService;
    public readonly IBoardService _boardService = boardService;
    public readonly IBoardCardService _boardCardService = boardCardService;
    public readonly IMessageService _messageService = messageService;

    [HttpPost("games")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> CreateGame(Game game)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            int gameId = await _gameService.CreateGame(playerId, game);

            return Ok($"Game {gameId} Created!");
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

    [HttpDelete("games/{gameId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> DeleteGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.DeleteGame(playerId, gameId);

            return Ok("Game Deleted!");
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

    [HttpPost("games/{gameId}/boards")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> CreateBoardCards(int gameId, [FromBody] IEnumerable<int> cardIds)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _boardCardService.CreateBoardCards(playerId, gameId, cardIds);

            return Ok("BoardCards Created!");
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

    [HttpDelete("games/{gameId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult> GetBoardWithBoardCards(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            Board board = await _boardService.GetBoardWithBoardCards(playerId, gameId);

            return Ok(board);
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
