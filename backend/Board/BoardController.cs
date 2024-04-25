using Microsoft.AspNetCore.Mvc;
using BoardCardEntity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BoardEntity;

[ApiController]
[Route("api/[controller]")]
public class BoardController(ILogger<BoardController> logger, IBoardService boardService, IBoardCardService boardCardService) : ControllerBase
{
    public readonly ILogger<BoardController> _logger = logger;
    public readonly IBoardService _boardService = boardService;
    public readonly IBoardCardService _boardCardService = boardCardService;

    [HttpPost]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> CreateBoard(Board board)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            int boardId = await _boardService.CreateBoard(playerId, board);

            return Ok(boardId);
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

    [HttpDelete("{boardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> DeleteBoard(int boardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _boardService.DeleteBoard(playerId, boardId);

            return Ok("Board Deleted!");
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

    [HttpPut("boards/{boardId}/boardcards/{boardcardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> SetPlayingBoardCard(int boardId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _boardService.ChooseCard(playerId, boardId, boardCardId);

            return Ok("BoardCard Chosen!");
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

    [HttpPut("boards/{boardId}")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdatePlayersLeftOnBoard(int boardId, [FromBody] int playersLeft)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _boardService.UpdatePlayersLeft(playerId, boardId, playersLeft);

            return Ok("Updated Players Left!");
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

    [HttpPost("boards/{boardId}/boardcards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> CreateBoardCards(int boardId, IEnumerable<int> cardIds)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _boardCardService.CreateBoardCards(playerId, boardId, cardIds);

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

    [HttpPut("boards/{boardId}/boardcards")]
    [Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult> UpdateBoardCardsActivity(int boardId, [FromBody] IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        try
        {
            // TODO
            int playerId = ParsePlayerIdClaim();
            await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates);

            return Ok("BoardCard Activity Updated!");
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
