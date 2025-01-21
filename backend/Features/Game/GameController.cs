using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Message;
using Backend.Features.Shared.Common;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

[ApiController]
[Route("api/[controller]")]
public class GameController(ILogger<GameController> logger, IGameService gameService, IBoardService boardService, IBoardRepository boardRepository) : ControllerBase
{
    private readonly ILogger<GameController> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IBoardRepository _boardRepository = boardRepository;
    private readonly IBoardService _boardService = boardService;

    [HttpPost("games/{gameState}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<int>> CreateGame(GameState gameState)
    {
        try
        {
            var playerId = TokenExtractor.ParsePlayerIdClaim(User);
            var gameRes = await _gameService.CreateGame(playerId, gameState);
            if (gameRes.IsError)
                return BadRequest(gameRes.Message);

            var gameId = gameRes.Data;
            var boardRes = await _boardRepository.Create(new BoardEntity(playerId, gameId));
            return boardRes.Resolve(
                suc => Ok(gameId),
                err => BadRequest(err.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateGame)");
            return StatusCode(500);
        }
    }

    [HttpDelete("games/{gameId:int}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> DeleteGame(int gameId)
    {
        try
        {
            var playerId = TokenExtractor.ParsePlayerIdClaim(User);
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

    [HttpGet("games/{gameId:int}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> GetBoardWithBoardCards(int gameId)
    {
        try
        {
            var playerId = TokenExtractor.ParsePlayerIdClaim(User);
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
}