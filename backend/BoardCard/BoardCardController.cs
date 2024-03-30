using Microsoft.AspNetCore.Mvc;
using BoardCardEntity;
using System;

namespace BoardCardEntity;

[ApiController]
[Route("api/[controller]")]
public class BoardCardController(IBoardCardService boardcardService) : ControllerBase
{
    public readonly IBoardCardService _boardcardService = boardcardService;
}