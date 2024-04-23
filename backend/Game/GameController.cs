using Microsoft.AspNetCore.Mvc;
using BoardEntity;
using BoardCardEntity;
using MessageEntity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Enum;

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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            int gameId = await _gameService.CreateGame(int.Parse(userIdClaim!), game);
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
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> DeleteGame(int gameId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _gameService.DeleteGame(int.Parse(userIdClaim!), gameId);
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

    [HttpPut("games/{gameId}/leave")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> LeaveGame(int gameId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            // TODO
            return Ok("Player Left The Game!");
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

    [HttpPut("games/{gameId}/join")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> JoinGame(int gameId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _gameService.JoinGameById(int.Parse(userIdClaim!), gameId);
            return Ok($"Player Joined Game {gameId}!");
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


    [HttpPut("games/{gameId}/update-state")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdateGameState(int gameId, [FromBody] State state)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _gameService.UpdateGameState(int.Parse(userIdClaim!), gameId, state);
            return Ok("Game State And Player Turn Updated!");
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

    [HttpPut("games/{gameId}/update-turn")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdateCurrentPlayerTurn(int gameId, [FromBody] int playerNumber)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _gameService.UpdateCurrentPlayerTurn(int.Parse(userIdClaim!), gameId, playerNumber);
            return Ok("Game State And Player Turn Updated!");
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

    [HttpPost("games/{gameId}/messages")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> SendMessage(int gameId, [FromBody] Message message)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _messageService.CreateMessage(int.Parse(userIdClaim!), gameId, message);
            return Ok("Message Sendt!");
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
}
