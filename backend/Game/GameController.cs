using Microsoft.AspNetCore.Mvc;
using BoardEntity;
using BoardCardEntity;
using MessageEntity;

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

    // Create Game
    // Delete Game
    // Get All Games / Get Game History ??
    // Leave Game
    // Update Game State (Maybe create just one for theese two)
    // Update Player Turn (Maybe create just one for theese two)

    // Create Board
    // Choose Playing Card
    // Update Players Left

    // Create Board Cards 
    // Update Active Card
}
