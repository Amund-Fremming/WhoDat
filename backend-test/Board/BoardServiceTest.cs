using Xunit;
using Moq;
using Data;
using BoardEntity;
using BoardCardEntity;
using GameEntity;
using PlayerEntity;
using Enum;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

public class BoardServiceTests
{
    public readonly BoardService _boardService;
    public readonly Mock<AppDbContext> _mockContext;
    public readonly Mock<BoardRepository> _mockBoardRepository;
    public readonly Mock<BoardCardRepository> _mockBoardCardRepository;
    public readonly Mock<GameRepository> _mockGameRepository;
    public readonly Mock<PlayerRepository> _mockPlayerRepository;
    public readonly Mock<ILogger<BoardService>> _mockLogger;

    public BoardServiceTests()
    {
        // Create the mock objects
        _mockContext = new Mock<AppDbContext>();
        _mockLogger = new Mock<ILogger<BoardService>>();
        _mockBoardRepository = new Mock<BoardRepository>();
        _mockBoardCardRepository = new Mock<BoardCardRepository>();
        _mockGameRepository = new Mock<GameRepository>();
        _mockPlayerRepository = new Mock<PlayerRepository>();

        // Create the service with the mocked dependencies
        _boardService = new BoardService(
            _mockLogger.Object,
            _mockContext.Object,
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

