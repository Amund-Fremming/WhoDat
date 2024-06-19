namespace GalleryEntityTest;

public class GalleryServiceTest
{
    public readonly Mock<ILogger<IGalleryService>> _mockLogger;
    public readonly Mock<IGalleryRepository> _mockGalleryRepository;
    public readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly IGalleryService _galleryService;
    private readonly AppDbContext _context;

    public GalleryServiceTest()
    {
        _mockLogger = new Mock<ILogger<IGalleryService>>();
        _mockGalleryRepository = new Mock<IGalleryRepository>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _galleryService = new GalleryService(
                _context,
                _mockLogger.Object,
                _mockGalleryRepository.Object,
                _mockPlayerRepository.Object
                );
    }

    [Fact]
    public async Task CreateGallery_Successful_ReturnsGalleryId()
    {
        int playerId = 12;
        string galleryName = "GalleryOne";
        int expectedId = 2;

        Player player = new Player("", "", "", Enum.Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(playerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.CreateGallery(It.Is<Gallery>(g => g.PlayerID == playerId && g.Name == galleryName)))
            .ReturnsAsync(expectedId);

        int result = await _galleryService.CreateGallery(playerId, gallery);

        Assert.Equal(expectedId, result);
        _mockGalleryRepository.Verify(repo => repo.CreateGallery(It.IsAny<Gallery>()), Times.Once);
    }

    [Fact]
    public async Task CreateGallery_PlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        string galleryName = "GalleryOne";
        Gallery gallery = new Gallery(playerId, galleryName);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _galleryService.CreateGallery(playerId, gallery));
    }

    [Fact]
    public async Task CreateGallery_GalleryIsNull_ShouldThrow()
    {
        int playerId = 12;
        Gallery? gallery = null;

        Player player = new Player("", "", "", Enum.Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        await Assert.ThrowsAsync<ArgumentNullException>(() => _galleryService.CreateGallery(playerId, gallery!));
    }

    [Fact]
    public async Task DeleteGallery_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "GalleryOne";

        Player player = new Player("", "", "", Enum.Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(playerId, galleryName);
        gallery.GalleryID = galleryId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        _mockGalleryRepository.Setup(repo => repo.DeleteGallery(gallery))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _galleryService.DeleteGallery(playerId, galleryId);

        _mockGalleryRepository.Verify(repo => repo.DeleteGallery(gallery), Times.Once);
    }

    [Fact]
    public async Task DeleteGallery_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 22;
        int galleryId = 33;
        string galleryName = "GalleryOne";

        Player player = new Player("", "", "", Enum.Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(ownerPlayerId, galleryName);
        gallery.GalleryID = galleryId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _galleryService.DeleteGallery(playerId, galleryId));
    }

    [Fact]
    public async Task DeleteGallery_WhenPlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _galleryService.DeleteGallery(playerId, galleryId));
    }

    [Fact]
    public async Task DeleteGallery_WhenGalleryDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;

        Player player = new Player("", "", "", Enum.Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ThrowsAsync(new KeyNotFoundException($"Gallery with id {galleryId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _galleryService.DeleteGallery(playerId, galleryId));
    }

    [Fact]
    public async Task DeleteGallery_Successful_PlayerIsAdminAndNotOwner()
    {
        int playerId = 12;
        int ownerPlayerId = 22;
        int galleryId = 33;
        string galleryName = "GalleryOne";

        Player player = new Player("", "", "", Enum.Role.ADMIN);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(ownerPlayerId, galleryName);
        gallery.GalleryID = galleryId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        _mockGalleryRepository.Setup(repo => repo.DeleteGallery(gallery))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _galleryService.DeleteGallery(playerId, galleryId);

        _mockGalleryRepository.Verify(repo => repo.DeleteGallery(gallery), Times.Once);
    }
}
