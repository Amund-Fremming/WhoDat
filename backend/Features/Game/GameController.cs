using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Message;

namespace Backend.Features.Game;

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
    public async Task<ActionResult<int>> CreateGame(GameEntity game)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            int gameId = await _gameService.CreateGame(playerId, game);
            await _boardService.CreateBoard(playerId, gameId);

            return Ok(gameId);
        }
        catch (Exception e)
        {
            return HandleException(e);
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
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpGet("games/{gameId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> GetBoardWithBoardCards(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            BoardEntity board = await _boardService.GetBoardWithBoardCards(playerId, gameId);

            return Ok(board);
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