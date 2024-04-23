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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            int boardId = await _boardService.CreateBoard(int.Parse(userIdClaim!), board);
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _boardService.DeleteBoard(int.Parse(userIdClaim!), boardId);
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _boardService.ChooseCard(int.Parse(userIdClaim!), boardId, boardCardId);
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _boardService.UpdatePlayersLeft(int.Parse(userIdClaim!), boardId, playersLeft);
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
    public async Task<ActionResult> CreateBoardCards(int boardId, List<int> cardIds)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await _boardCardService.CreateBoardCards(int.Parse(userIdClaim!), boardId, cardIds);
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
    public async Task<ActionResult> UpdateBoardCardsActivity(int boardId, [FromBody] BoardCardUpdateDto boardCardUpdateDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        try
        {
            // TODO
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
}
