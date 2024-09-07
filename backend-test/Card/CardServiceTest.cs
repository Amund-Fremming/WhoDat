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
    public readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    public readonly ICardService _cardService;
    public readonly AppDbContext _context;

    public CardServiceTest()
    {
        _mockLogger = new Mock<ILogger<ICardService>>();
        _mockCardRepository = new Mock<ICardRepository>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _cardService = new CardService(
                _mockLogger.Object,
                _mockCardRepository.Object,
                _mockHttpClientFactory.Object
                );
    }

    [Fact]
    public async Task CreateCard_ArgumentNull_ShouldThrow()
    {
        int playerId = 12;
        string cardName = "Bison";
        CardInputDto dto = new CardInputDto() { Name = cardName, Image = null };

        await Assert.ThrowsAsync<ArgumentNullException>(() => _cardService.CreateCard(playerId, dto));
    }

    ///

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
        var expectedCards = new List<Card>
        {
            new Card { CardID = 1, PlayerID =  playerId },
            new Card { CardID = 2, PlayerID =  playerId }
        };

        _mockCardRepository.Setup(repo => repo.GetAllCards(playerId))
            .ReturnsAsync(expectedCards);

        var result = await _cardService.GetAllCards(playerId);

        Assert.Equal(expectedCards, result);
        _mockCardRepository.Verify(repo => repo.GetAllCards(playerId), Times.Once);
    }
}
