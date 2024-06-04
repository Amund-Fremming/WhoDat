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

    [Fact]
    public async Task CreatePlayer_Successful()
    {
    }

    [Fact]
    public async Task CreatePlayer_UsernameNotAvailable_ShouldThrow()
    {
    }

    ///

    [Fact]
    public async Task DeletePlayer_Successful_PlayerExists()
    {
    }

    [Fact]
    public async Task DeletePlayer_PlayerDoesNotExists_ShouldThrow()
    {
    }

    ///

    [Fact]
    public async Task UpdateUsername_Successful()
    {
    }

    [Fact]
    public async Task UpdateUsername_PlayerDoesNotExist_ShouldThrow()
    {
    }

    [Fact]
    public async Task UpdateUsername_UsernameExists_ShouldThrow()
    {
    }

    ///

    [Fact]
    public async Task UpdatePassword_Successful()
    {
    }

    [Fact]
    public async Task UpdatePassword_PlayerDoesNotExist_ShouldThrow()
    {
    }
}
