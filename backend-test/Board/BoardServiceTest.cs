using Xunit;
using Moq;
using Data;
using BoardEntity;
using BoardCardEntity;
using GameEntity;
using PlayerEntity;
using Enum;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class BoardServiceTests
{
    private readonly BoardService _boardService;
    private readonly Mock<BoardRepository> _mockBoardRepository;
    private readonly Mock<BoardCardRepository> _mockBoardCardRepository;
    private readonly Mock<GameRepository> _mockGameRepository;
    private readonly Mock<PlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<BoardService>> _mockLogger;
    private readonly AppDbContext _context;

    public BoardServiceTests()
    {
        // Set up in-memory database for context
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new AppDbContext(options);

        // Create the mock objects
        _mockLogger = new Mock<ILogger<BoardService>>();

        // Create instances of repositories with the in-memory context
        _mockBoardRepository = new Mock<BoardRepository>(_context);
        _mockBoardCardRepository = new Mock<BoardCardRepository>(_context);
        _mockGameRepository = new Mock<GameRepository>(_context);
        _mockPlayerRepository = new Mock<PlayerRepository>(_context);

        // Create the service with the mocked dependencies
        _boardService = new BoardService(
            _mockLogger.Object,
            _context,
            _mockBoardRepository.Object,
            _mockBoardCardRepository.Object,
            _mockGameRepository.Object,
            _mockPlayerRepository.Object
        );
    }

    [Fact]
    public async Task CreateBoard_Successful_ReturnsBoardId()
    {
        // Arrange
        int playerId = 1;
        int gameId = 123;
        int boardId = 456;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId)).ReturnsAsync(new Player("", "", "", Role.USER));
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId)).ReturnsAsync(new Game(playerId, State.BOTH_CHOSING_CARDS));
        _mockBoardRepository.Setup(repo => repo.CreateBoard(It.IsAny<Board>())).ReturnsAsync(boardId);

        // Act
        int result = await _boardService.CreateBoard(playerId, gameId);

        // Assert
        Assert.Equal(boardId, result);
        _mockPlayerRepository.Verify(repo => repo.GetPlayerById(playerId), Times.Once);
        _mockGameRepository.Verify(repo => repo.GetGameById(gameId), Times.Once);
        _mockBoardRepository.Verify(repo => repo.CreateBoard(It.IsAny<Board>()), Times.Once);
    }
}

