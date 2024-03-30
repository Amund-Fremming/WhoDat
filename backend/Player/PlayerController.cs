using Microsoft.AspNetCore.Mvc;
using PlayerEntity;
using System;

namespace PlayerEntity;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IPlayerService playerService) : ControllerBase
{
    public readonly IPlayerService _playerService = playerService;
}