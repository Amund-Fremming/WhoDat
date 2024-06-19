namespace GameEntityTest;

public class GameServiceTest
{
    public readonly AppDbContext _context;
    public readonly Mock<ILogger<IGameService>> _mockLogger;
    public readonly Mock<IGameRepository> _mockGameRepository;
    public readonly Mock<IPlayerRepository> _mockPlayerRepository;
    public readonly IGameService _gameService;

    public GameServiceTest()
    {
        _mockLogger = new Mock<ILogger<IGameService>>();
        _mockGameRepository = new Mock<IGameRepository>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _gameService = new GameService(
                _context,
                _mockLogger.Object,
                _mockGameRepository.Object,
                _mockPlayerRepository.Object
                );
    }

    // CreateGame

    ///

    // DeleteGame

    ///

    // JoinGameById

    ///

    // LeaveGameById

    ///

    // UpdateGameState

    ///

    // GetRecentGamePlayed

    ///

    // StartGame
}
