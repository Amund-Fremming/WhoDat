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
    [Authorize(Roles = "ADMIN,USER")]
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

    [HttpPut("games/{gameId}/leave")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> LeaveGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.LeaveGameById(playerId, gameId);

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
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.JoinGameById(playerId, gameId);

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
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.UpdateGameState(playerId, gameId, state);

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
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.UpdateCurrentPlayerTurn(playerId, gameId, playerNumber);

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
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _messageService.CreateMessage(playerId, gameId, message);

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

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
}
