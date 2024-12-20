using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Message;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

[ApiController]
[Route("api/[controller]")]
public class GameController(ILogger<GameController> logger, IGameService gameService, IBoardService boardService, IBoardRepository boardRepository, IBoardCardService boardCardService, IMessageService messageService) : ControllerBase
{
    private readonly ILogger<GameController> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IBoardRepository _boardRepository = boardRepository;
    private readonly IBoardService _boardService = boardService;
    private readonly IBoardCardService _boardCardService = boardCardService;
    private readonly IMessageService _messageService = messageService;

    [HttpPost("games")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<int>> CreateGame(CreateGameRequest gameRequest)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            gameRequest.PlayerOneID = playerId;
            var gameRes = await _gameService.CreateGame(playerId, gameRequest);
            if (gameRes.IsError)
                return BadRequest(gameRes.Message);

            var gameId = gameRes.Data;
            var boardRes = await _boardRepository.Create(new BoardEntity(playerId, gameId));
            return boardRes.Resolve(
                suc => Ok(suc.Data),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateGame)");
            return StatusCode(500);
        }
    }

    [HttpDelete("games/{gameId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> DeleteGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            var result = await _gameService.DeleteGame(playerId, gameId);
            return result.Resolve(
                suc => Ok(),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteGame)");
            return StatusCode(500);
        }
    }

    [HttpGet("games/{gameId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> GetBoardWithBoardCards(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            var result = await _boardService.GetBoardWithBoardCards(playerId, gameId);
            return result.Resolve(
                suc => Ok(suc.Data),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBoardWithBoardCards)");
            return StatusCode(500);
        }
    }

    [NonAction]
    private int ParsePlayerIdClaim() => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
}