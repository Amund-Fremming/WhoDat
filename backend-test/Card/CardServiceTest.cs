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

    /*
     * TEST - todo
     *
     * Create success
     * Create null argument
     * Create player not exist
     * Create gallery not exist
     *
     * Delete success
     * Delete card not exist
     * Delete gallery not exist
     * Delete Player has not permission
     */

    [Fact]
    public async Task CreateCard_Successful_CreateCard()
    {
    }
}
