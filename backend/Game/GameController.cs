using Microsoft.AspNetCore.Mvc;

namespace GameEntity;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    public readonly IGameService _gameService = gameService;
}
