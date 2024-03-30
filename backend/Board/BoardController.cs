using Microsoft.AspNetCore.Mvc;
using BoardEntity;
using System;

namespace BoardEntity;

[ApiController]
[Route("api/[controller]")]
public class BoardController(IBoardService boardService) : ControllerBase
{
    public readonly IBoardService _boardService = boardService;
}