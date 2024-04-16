using Microsoft.AspNetCore.Mvc;
using GalleryEntity;
using CardEntity;

namespace PlayerEntity;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(ILogger<PlayerController> logger, IPlayerService playerService, IGalleryService galleryService, ICardService cardService) : ControllerBase
{
    public readonly ILogger<PlayerController> _logger = logger;
    public readonly IPlayerService _playerService = playerService;
    public readonly IGalleryService _galleryService = galleryService;
    public readonly ICardService _cardService = cardService;

    // Delete Player
    // Update Player Username

    // Get Gallery
    // Create Gallery (Limit to one per now)

    // Add Card to Gallery
    // Update Card in Gallery
    // Delete Card in Gallery
}
