using GalleryEntity;

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
        // Arrange
        int playerId = 12;
        string galleryName = "GalleryOne";
        int expectedId = 2;

        Player player = new Player("", "", "", Enum.Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(playerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.CreateGallery(It.Is<Gallery>(g => g.PlayerID == playerId && g.Name == galleryName)))
            .ReturnsAsync(expectedId);

        // Act
        int result = await _galleryService.CreateGallery(playerId, gallery);

        // Assert
        Assert.Equal(expectedId, result);
        _mockGalleryRepository.Verify(repo => repo.CreateGallery(It.IsAny<Gallery>()), Times.Once);
    }

    [Fact]
    public async Task CreateGallery_Unsuccessful_PlayerDoesNotExist()
    {
        // Arrange
        int playerId = 12;
        string galleryName = "GalleryOne";
        Gallery gallery = new Gallery(playerId, galleryName);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _galleryService.CreateGallery(playerId, gallery));
    }

    [Fact]
    public async Task DeleteGallery_Successful_PlayerHasPermission()
    {
        // Arrange
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "GalleryOne";

        Player player = new Player("", "", "", Enum.Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Gallery gallery = new Gallery(playerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        // Act


        // Assert
    }

    [Fact]
    public async Task DeleteGallery_Unsuccessful_PlayerHasNotPermission()
    {
    }

    [Fact]
    public async Task DeleteGallery_WhenPlayerDoesNotExist_ShouldThrow()
    {
    }

    [Fact]
    public async Task DeleteGallery_WhenGalleryDoesNotExist_ShouldThrow()
    {
    }
}
