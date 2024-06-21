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

    // CreateGame_Successful_ReturnsId
    // CreateGame_PlayerDoesNotExist_ShouldThrow

    ///

    // DeleteGame_Successful_PlayerHasPermission
    // DeleteGame_GameDoesNotExist_ShouldThrow
    // DeleteGame_PlayerHasNotPermission_ShouldThrow

    ///

    // JoinGameById_Successful_ReturnsGame
    // JoinGameById_GameDoesNotExist_ShouldThrow
    // JoinGameById_GameIsFull_ShouldThrow
    // JoinGameById_PlayerDoesNotExist_ShouldThrow

    ///

    // LeaveGameById_Successful_
    // LeaveGameById_GameDoesNotExist_ShouldThrow
    // LeaveGameById_PlayerHasNotPermission_ShouldThrow

    ///

    // UpdateGameState_Successful_PlayerHasPermission
    // UpdateGameState_GameDoesNotExist_ShouldThrow

    ///

    // GetRecentGamePlayed_Successful
    // GetRecentGamePlayed_PlayerDoesNotExist_ShouldThrow

    ///

    // StartGame_Successful_ReturnsNewState
    // StartGame_PlayerDoesNotExist_ShouldThrow
    // StartGame_GameDoesNotExist_ShouldThrow
    // StartGame_PlayerHasNotPermission_ShouldThrow
    // StartGame_StateIsNotCorrect_ShouldThrow
    // StartGame_NotEnoughPlayers_ShouldThrow
    // StartGame_PlayerOneNotCreatedBoard_ShouldThrow
    // StartGame_PlayerTwoNotCreatedBoard_ShouldThrow
    // StartGame_PlayerOneNotChoosenPlayingCard_ShouldThrow
    // StartGame_PlayerTwoNotChoosenPlayingCard_ShouldThrow
}
