namespace PlayerEntityTest;

public class PlayerServiceTest
{
    public readonly Mock<ILogger<IPlayerService>> _mocklogger;
    public readonly Mock<IPlayerRepository> _mockPlayerRepository;
    public readonly Mock<IPasswordHasher<Player>> _mockpasswordHasher;
    public readonly IPlayerService _playerService;
    public readonly AppDbContext _context;

    public PlayerServiceTest()
    {
        _mocklogger = new Mock<ILogger<IPlayerService>>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockpasswordHasher = new Mock<IPasswordHasher<Player>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _playerService = new PlayerService(
                _mocklogger.Object,
                _mockPlayerRepository.Object,
                _mockpasswordHasher.Object
                );
    }
}
