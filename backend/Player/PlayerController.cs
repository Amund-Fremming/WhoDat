using Microsoft.AspNetCore.Mvc;

namespace PlayerEntity;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IPlayerService playerService) : ControllerBase
{
    public readonly IPlayerService _playerService = playerService;
}
