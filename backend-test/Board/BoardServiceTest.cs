namespace BoardEntityTest;

public class BoardServiceTests
{
    private readonly Mock<ILogger<BoardService>> _mockLogger;
    private readonly Mock<IBoardRepository> _mockBoardRepository;
    private readonly Mock<IBoardCardRepository> _mockBoardCardRepository;
    private readonly Mock<IGameRepository> _mockGameRepository;
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly BoardService _boardService;
    private readonly AppDbContext _context;

    public BoardServiceTests()
    {
        _mockLogger = new Mock<ILogger<BoardService>>();
        _mockBoardRepository = new Mock<IBoardRepository>();
        _mockBoardCardRepository = new Mock<IBoardCardRepository>();
        _mockGameRepository = new Mock<IGameRepository>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _boardService = new BoardService(
            _mockLogger.Object,
            _context,
            _mockBoardRepository.Object,
            _mockBoardCardRepository.Object,
            _mockGameRepository.Object,
            _mockPlayerRepository.Object
        );
    }

    //  DENNE MA SE OM GAME FINNES
    [Fact]
    public async Task CreateBoard_Successful_ReturnsBoardId()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int expectedBoardId = 1;

        Board board = new Board(playerId, gameId);

        _mockBoardRepository.Setup(repo => repo.CreateBoard(It.Is<Board>(b => b.PlayerID == playerId && b.GameID == gameId)))
                            .ReturnsAsync(expectedBoardId);

        // Act
        int result = await _boardService.CreateBoard(playerId, gameId);

        // Assert
        Assert.Equal(expectedBoardId, result);
        _mockBoardRepository.Verify(repo => repo.CreateBoard(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public async Task DeleteBoard_Successful_PlayerHasPermission()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int boardId = 1;

        Player player = new Player("", "", "", Enum.Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Board board = new Board(playerId, gameId);
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        _mockBoardRepository.Setup(repo => repo.DeleteBoard(board))
                                .Returns(Task.CompletedTask)
                                .Verifiable();

        // Act 
        await _boardService.DeleteBoard(playerId, boardId);

        // Assert
        _mockBoardRepository.Verify(repo => repo.DeleteBoard(board), Times.Once());
    }

    [Fact]
    public async Task DeleteBoard_Unsuccessful_PlayerHasNotPermission()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 55;
        int gameId = 22;
        int boardId = 1;

        Player player = new Player("", "", "", Enum.Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Board board = new Board(ownerPlayerId, gameId);
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.DeleteBoard(playerId, boardId));
    }

    [Fact]
    public async Task DeleteBoard_WhenBoardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int boardId = 1;

        var player = new Player("", "", "", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId} does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.DeleteBoard(playerId, boardId));
    }
}

