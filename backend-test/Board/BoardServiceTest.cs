using Xunit;
using Moq;
using Data;
using BoardEntity;
using BoardCardEntity;
using GameEntity;
using PlayerEntity;
using Enum;

public class BoardServiceTest(BoardService boardService, Mock<AppDbContext> context, Mock<BoardRepository> boardRepository, Mock<BoardCardRepository> boardCardRepository, Mock<GameRepository> gameRepository, Mock<PlayerRepository> playerRepository)
{
    public readonly BoardService _boardService = boardService;
    public readonly Mock<AppDbContext> _mockContext = context;
    public readonly Mock<BoardRepository> _mockBoardRepository = boardRepository;
    public readonly Mock<BoardCardRepository> _mockBoardCardRepository = boardCardRepository;
    public readonly Mock<GameRepository> _mockGameRepository = gameRepository;
    public readonly Mock<PlayerRepository> _mockPlayerRepository = playerRepository;

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
        _mockBoardRepository.Verify(repo => repo.GetBoardById(boardId), Times.Once);

    }
}
