namespace BoardCardEntityTest;

public class BoardCardTest
{
    public readonly Mock<ILogger<IBoardCardService>> _mockLogger;
    public readonly Mock<IBoardCardRepository> _mockBoardCardRepository;
    public readonly Mock<IBoardRepository> _mockBoardRepository;
    public readonly Mock<ICardRepository> _mockCardRepository;
    public readonly Mock<IGameRepository> _mockGameRepository;
    public readonly IBoardCardService _boardCardService;
    public readonly AppDbContext _context;

    public BoardCardTest()
    {
        _mockLogger = new Mock<ILogger<IBoardCardService>>();
        _mockBoardCardRepository = new Mock<IBoardCardRepository>();
        _mockBoardRepository = new Mock<IBoardRepository>();
        _mockCardRepository = new Mock<ICardRepository>();
        _mockGameRepository = new Mock<IGameRepository>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _boardCardService = new BoardCardService(
                _context,
                _mockLogger.Object,
                _mockBoardCardRepository.Object,
                _mockBoardRepository.Object,
                _mockCardRepository.Object,
                _mockGameRepository.Object
                );
    }

    // CreateBoardCards_Successful_PlayerHasPermission
    // CreateBoardCards_GameDoesNotExist_ShouldThrow
    // CreateBoardCards_PlayerHasNotPermission_ShouldThrow
    // CreateBoardCards_InvalidPlayerPermission_ShouldThrow WITH ALL CASES

    ///

    // TODO
    /*
    public async Task UpdateBoardCardsActivity_Successful_PlayerHasPermission()
    {
        // Arrange
        int playerId = 12;
        int boardId = 22;
        int gameId = 33;
        IEnumerable<BoardCardUpdate> boardCardUpdates = Enumerable.Empty<BoardCardUpdate>();

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardsFromBoard(boardId))
            .ReturnsAsync();

        _mockBoardCardRepository.Setup(repo => repo.UpdateBoardCardsActivity(updateMap, boardCards))
            .ReturnsAsync();

        _mockBoardCardRepository.Setup(repo => repo.UpdateBoardCardsLeft(board, boardcardsLeft))
            .ReturnsAsync();

        // Act
        int result = await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates);

        // Assert
        Assert.Equal(boardCardsLeft, result);
    }
    */

    public async Task UpdateBoardCardsActivity_BoardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int boardId = 22;
        int gameId = 33;
        IEnumerable<BoardCardUpdate> boardCardUpdates = Enumerable.Empty<BoardCardUpdate>();

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates));
    }

    public async Task UpdateBoardCardsActivity_PlayerHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 55;
        int boardId = 22;
        int gameId = 33;
        IEnumerable<BoardCardUpdate> boardCardUpdates = Enumerable.Empty<BoardCardUpdate>();

        Board board = new Board(ownerPlayerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates));
    }

    ///

    public async Task GetBoardCardsFromBoard_Successful_PlayerHasPermission()
    {
        // Arrange
        int playerId = 12;
        int boardId = 22;
        int gameId = 33;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        IList<BoardCard> boardCards = new List<BoardCard>
        {
            new BoardCard(),
            new BoardCard(),
            new BoardCard(),
        };
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardsFromBoard(boardId))
            .ReturnsAsync(boardCards);

        // Act
        IEnumerable<BoardCard> result = await _boardCardService.GetBoardCardsFromBoard(playerId, boardId);

        // Assert
        Assert.Equal(boardCards, result.ToList());
    }

    public async Task GetBoardCardsFromBoard_BoardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int boardId = 22;

        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardCardService.GetBoardCardsFromBoard(playerId, boardId));
    }

    public async Task GetBoardCardsFromBoard_PlayerHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 13;
        int boardId = 22;
        int gameId = 33;

        Board board = new Board(ownerPlayerId, gameId);
        board.PlayerID = ownerPlayerId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardCardService.GetBoardCardsFromBoard(playerId, boardId));
    }
}
