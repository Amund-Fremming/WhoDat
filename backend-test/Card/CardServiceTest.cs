/*
 * INFO
 *
 * UploadImageToCloudflare are manually tested
 * This method made the successful test on CreateCard not implemented
 */

namespace CardEntityTest;

public class CardServiceTest
{
    public readonly Mock<ILogger<ICardService>> _mockLogger;
    public readonly Mock<ICardRepository> _mockCardRepository;
    public readonly Mock<IGalleryRepository> _mockGalleryRepository;
    public readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    public readonly ICardService _cardService;
    public readonly AppDbContext _context;

    public CardServiceTest()
    {
        _mockLogger = new Mock<ILogger<ICardService>>();
        _mockCardRepository = new Mock<ICardRepository>();
        _mockGalleryRepository = new Mock<IGalleryRepository>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _cardService = new CardService(
                _mockLogger.Object,
                _mockCardRepository.Object,
                _mockGalleryRepository.Object,
                _mockHttpClientFactory.Object
                );
    }

    [Fact]
    public async Task CreateCard_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 2;
        int galleryId = 33;
        string galleryName = "Gallery";
        Card card = new Card(galleryId);
        card.GalleryID = galleryId;
        CardInputDto dto = new CardInputDto() { Card = card, Image = null };

        Gallery gallery = new Gallery(ownerPlayerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _cardService.CreateCard(playerId, dto));
    }

    [Fact]
    public async Task CreateCard_ArgumentNull_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "Gallery";
        Card card = new Card(galleryId);
        card.GalleryID = galleryId;
        CardInputDto dto = new CardInputDto() { Card = card, Image = null };

        Gallery gallery = new Gallery(playerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        await Assert.ThrowsAsync<ArgumentNullException>(() => _cardService.CreateCard(playerId, dto));
    }

    [Fact]
    public async Task CreateCard_GalleryDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;
        Card card = new Card(galleryId);
        card.GalleryID = galleryId;
        CardInputDto dto = new CardInputDto() { Card = card, Image = null };

        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ThrowsAsync(new KeyNotFoundException($"Gallery with id {galleryId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.CreateCard(playerId, dto));
    }

    ///

    [Fact]
    public async Task DeleteCard_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "Gallery";
        int cardId = 3;

        Card card = new Card(galleryId);
        _mockCardRepository.Setup(repo => repo.GetCardById(cardId))
            .ReturnsAsync(card);

        Gallery gallery = new Gallery(playerId, galleryName);
        gallery.PlayerID = playerId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        _mockCardRepository.Setup(repo => repo.DeleteCard(card))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _cardService.DeleteCard(playerId, cardId);

        _mockCardRepository.Verify(repo => repo.DeleteCard(card), Times.Once);
    }

    [Fact]
    public async Task DeleteCard_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 1;
        int galleryId = 33;
        string galleryName = "Gallery";
        int cardId = 3;

        Card card = new Card(galleryId);
        _mockCardRepository.Setup(repo => repo.GetCardById(cardId))
            .ReturnsAsync(card);

        Gallery gallery = new Gallery(playerId, galleryName);
        gallery.PlayerID = ownerPlayerId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _cardService.DeleteCard(playerId, cardId));
    }

    [Fact]
    public async Task DeleteCard_GalleryDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;
        int cardId = 3;

        Card card = new Card(galleryId);
        _mockCardRepository.Setup(repo => repo.GetCardById(cardId))
            .ReturnsAsync(card);

        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ThrowsAsync(new KeyNotFoundException($"Gallery with id {galleryId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.DeleteCard(playerId, cardId));
    }

    [Fact]
    public async Task DeleteCard_CardDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;
        int cardId = 3;

        Card card = new Card(galleryId);
        _mockCardRepository.Setup(repo => repo.GetCardById(cardId))
            .ThrowsAsync(new KeyNotFoundException($"Card with id {cardId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.DeleteCard(playerId, cardId));
    }

    ///

    [Fact]
    public async Task GetAllCards_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "Gallery";
        var expectedCards = new List<Card>
        {
            new Card { CardID = 1, GalleryID = galleryId },
            new Card { CardID = 2, GalleryID = galleryId }
        };

        Gallery gallery = new Gallery(playerId, galleryName);
        gallery.GalleryID = galleryId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        _mockCardRepository.Setup(repo => repo.GetAllCards(galleryId))
            .ReturnsAsync(expectedCards);

        var result = await _cardService.GetAllCards(playerId, galleryId);

        Assert.Equal(expectedCards, result);
        _mockCardRepository.Verify(repo => repo.GetAllCards(galleryId), Times.Once);
    }

    [Fact]
    public async Task GetAllCards_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 2;
        int galleryId = 33;
        string galleryName = "Gallery";

        Gallery gallery = new Gallery(ownerPlayerId, galleryName);
        gallery.GalleryID = galleryId;
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ReturnsAsync(gallery);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _cardService.GetAllCards(playerId, galleryId));
    }

    [Fact]
    public async Task GetAllCards_GalleryDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int galleryId = 33;
        string galleryName = "Gallery";

        Gallery gallery = new Gallery(playerId, galleryName);
        _mockGalleryRepository.Setup(repo => repo.GetGalleryById(galleryId))
            .ThrowsAsync(new KeyNotFoundException($"Gallery with id {galleryId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.GetAllCards(playerId, galleryId));
    }
}
