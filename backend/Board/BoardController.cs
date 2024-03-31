using Microsoft.AspNetCore.Mvc;

namespace BoardEntity;

[ApiController]
[Route("api/[controller]")]
public class BoardController(IBoardService boardService) : ControllerBase
{
    public readonly IBoardService _boardService = boardService;
}
