namespace BoardCardEntityTest;

public class BoardCardServiceTest
{
    public readonly Mock<ILogger<IBoardCardService>> _mockLogger;
    public readonly Mock<IBoardCardRepository> _mockBoardCardRepository;
    public readonly Mock<IBoardRepository> _mockBoardRepository;
    public readonly Mock<ICardRepository> _mockCardRepository;
    public readonly Mock<IGameRepository> _mockGameRepository;
    public readonly IBoardCardService _boardCardService;
    public readonly AppDbContext _context;

    public BoardCardServiceTest()
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

    [Fact]
    public async Task CreateBoardCards_Successful_PlayerHasPermission()
    {
    }

    [Fact]
    public async Task CreateBoardCards_GameDoesNotExist_ShouldThrow()
    {
    }

    [Fact]
    public async Task CreateBoardCards_PlayerHasNotPermission_ShouldThrow()
    {
    }

    // CreateBoardCards_InvalidPlayerPermission_ShouldThrow WITH ALL CASES

    ///

    [Fact]
    public async Task UpdateBoardCardsActivity_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int boardId = 22;
        int gameId = 33;
        int boardCardsLeft = 13;

        IEnumerable<BoardCardUpdate> boardCardUpdates = new List<BoardCardUpdate>
        {
            new BoardCardUpdate{ BoardCardID = 1, Active = true },
            new BoardCardUpdate{ BoardCardID = 2, Active = true },
            new BoardCardUpdate{ BoardCardID = 3, Active = true },
            new BoardCardUpdate{ BoardCardID = 4, Active = true },
            new BoardCardUpdate{ BoardCardID = 5, Active = true },
            new BoardCardUpdate{ BoardCardID = 6, Active = true },
            new BoardCardUpdate{ BoardCardID = 7, Active = true },
            new BoardCardUpdate{ BoardCardID = 8, Active = true },
            new BoardCardUpdate{ BoardCardID = 9, Active = true },
            new BoardCardUpdate{ BoardCardID = 10, Active = true },
            new BoardCardUpdate{ BoardCardID = 11, Active = true },
            new BoardCardUpdate{ BoardCardID = 12, Active = true },
            new BoardCardUpdate{ BoardCardID = 13, Active = true },
            new BoardCardUpdate{ BoardCardID = 14, Active = false },
            new BoardCardUpdate{ BoardCardID = 15, Active = false },
            new BoardCardUpdate{ BoardCardID = 16, Active = false },
            new BoardCardUpdate{ BoardCardID = 17, Active = false },
            new BoardCardUpdate{ BoardCardID = 18, Active = false },
            new BoardCardUpdate{ BoardCardID = 19, Active = false },
            new BoardCardUpdate{ BoardCardID = 20, Active = false },
        };

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        BoardCard b1 = new BoardCard(boardId, 1);
        BoardCard b2 = new BoardCard(boardId, 2);
        BoardCard b3 = new BoardCard(boardId, 3);
        BoardCard b4 = new BoardCard(boardId, 4);
        BoardCard b5 = new BoardCard(boardId, 5);
        BoardCard b6 = new BoardCard(boardId, 6);
        BoardCard b7 = new BoardCard(boardId, 7);
        b1.Active = false;
        b2.Active = false;
        b3.Active = false;
        b4.Active = false;
        b5.Active = false;
        b6.Active = false;
        b7.Active = false;

        IList<BoardCard> boardCards = new List<BoardCard>
        {
            b1, b2, b3, b4, b5, b6, b7,
            new BoardCard(boardId, 8),
            new BoardCard(boardId, 9),
            new BoardCard(boardId, 10),
            new BoardCard(boardId, 11),
            new BoardCard(boardId, 12),
            new BoardCard(boardId, 13),
            new BoardCard(boardId, 14),
            new BoardCard(boardId, 15),
            new BoardCard(boardId, 16),
            new BoardCard(boardId, 17),
            new BoardCard(boardId, 18),
            new BoardCard(boardId, 19),
            new BoardCard(boardId, 20),
        };
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardsFromBoard(boardId))
            .ReturnsAsync(boardCards);

        int result = await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates);

        Assert.Equal(boardCardsLeft, result);
    }

    [Fact]
    public async Task UpdateBoardCardsActivity_BoardDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int boardId = 22;
        IEnumerable<BoardCardUpdate> boardCardUpdates = Enumerable.Empty<BoardCardUpdate>();

        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates));
    }

    [Fact]
    public async Task UpdateBoardCardsActivity_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 55;
        int boardId = 22;
        int gameId = 33;
        IEnumerable<BoardCardUpdate> boardCardUpdates = Enumerable.Empty<BoardCardUpdate>();

        Board board = new Board(ownerPlayerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates));
    }

    ///

    [Fact]
    public async Task GetBoardCardsFromBoard_Successful_PlayerHasPermission()
    {
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

        IEnumerable<BoardCard> result = await _boardCardService.GetBoardCardsFromBoard(playerId, boardId);

        Assert.Equal(boardCards, result.ToList());
    }

    [Fact]
    public async Task GetBoardCardsFromBoard_BoardDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int boardId = 22;

        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardCardService.GetBoardCardsFromBoard(playerId, boardId));
    }

    [Fact]
    public async Task GetBoardCardsFromBoard_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 13;
        int boardId = 22;
        int gameId = 33;

        Board board = new Board(ownerPlayerId, gameId);
        board.PlayerID = ownerPlayerId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardCardService.GetBoardCardsFromBoard(playerId, boardId));
    }
}
