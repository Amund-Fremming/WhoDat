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

    [Fact]
    public async Task CreateBoard_Successful_ReturnsBoardId()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int expectedBoardId = 1;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockBoardRepository.Setup(repo => repo.CreateBoard(It.IsAny<Board>()))
                            .ReturnsAsync(expectedBoardId);

        // Act
        int result = await _boardService.CreateBoard(playerId, gameId);

        // Assert
        Assert.Equal(expectedBoardId, result);
        _mockBoardRepository.Verify(repo => repo.CreateBoard(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public async Task CreateBoard_PlayerDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.CreateBoard(playerId, gameId));
    }

    [Fact]
    public async Task CreateBoard_GameDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.CreateBoard(playerId, gameId));
    }

    ///

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
    public async Task DeleteBoard_BoardDoesNotExist_ShouldThrow()
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

    [Fact]
    public async Task DeleteBoard_PlayerHasNotPermission_ShouldThrow()
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

    ///

    [Fact]
    public async Task ChooseBoardCard_Successfull_IsPlayerOneAndBothChoosingCards()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;
        int cardId = 44;
        State currentState = State.BOTH_PICKING_PLAYER;
        State expectedState = State.P2_PICKING_PLAYER;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        Game game = new Game(playerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        BoardCard boardCard = new BoardCard(boardId, cardId);
        boardCard.BoardID = boardCardId;
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardById(boardCardId))
            .ReturnsAsync(boardCard);

        _mockBoardRepository.Setup(repo => repo.ChooseBoardCard(board, boardCard))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockGameRepository.Setup(repo => repo.UpdateGameState(game, expectedState))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        State result = await _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId);

        // Assert
        _mockBoardRepository.Verify(repo => repo.ChooseBoardCard(board, boardCard), Times.Once);
        _mockGameRepository.Verify(repo => repo.UpdateGameState(game, expectedState), Times.Once);

        Assert.Equal(expectedState, result);
    }

    [Fact]
    public async Task ChooseBoardCard_BoardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 44;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;
        State currentState = State.BOTH_PICKING_PLAYER;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        Game game = new Game(ownerPlayerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId));
    }

    [Fact]
    public async Task ChooseBoardCard_GameDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 44;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;

        Board board = new Board(ownerPlayerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId));
    }

    [Fact]
    public async Task ChooseBoardCard_PlayerHasNotBoardPermission_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 44;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;
        State currentState = State.BOTH_PICKING_PLAYER;

        Board board = new Board(ownerPlayerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        Game game = new Game(playerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId));
    }

    [Fact]
    public async Task ChooseBoardCard_PlayerHasNotGamePermission_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 44;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;
        State currentState = State.BOTH_PICKING_PLAYER;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        Game game = new Game(ownerPlayerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId));
    }

    [Fact]
    public async Task ChooseBoardCard_BoardCardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int boardId = 1;
        int boardCardId = 33;
        int cardId = 55;
        State currentState = State.BOTH_PICKING_PLAYER;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        Game game = new Game(playerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        BoardCard boardCard = new BoardCard(boardId, cardId);
        boardCard.BoardID = boardCardId;
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardById(boardCardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId));
    }

    // All Cases where its duccessful and state is updated correct

    ///

    [Fact]
    public async Task UpdateBoardCardsLeft_Successful_PlayerHasPermission()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int boardId = 1;
        int activePlayers = 18;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        _mockBoardRepository.Setup(repo => repo.UpdateBoardCardsLeft(board, activePlayers))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _boardService.UpdateBoardCardsLeft(playerId, boardId, activePlayers);

        // Assert
        _mockBoardRepository.Verify(repo => repo.UpdateBoardCardsLeft(board, activePlayers));
    }

    [Fact]
    public async Task UpdateBoardCardsLeft_BoardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;
        int boardId = 1;
        int activePlayers = 18;

        Board board = new Board(playerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ThrowsAsync(new KeyNotFoundException($"Board with id {boardId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.UpdateBoardCardsLeft(playerId, boardId, activePlayers));

    }

    [Fact]
    public async Task UpdateBoardCardsLeft_PlayerHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int ownerPlayerId = 44;
        int gameId = 22;
        int boardId = 1;
        int activePlayers = 18;

        Board board = new Board(ownerPlayerId, gameId);
        board.BoardID = boardId;
        _mockBoardRepository.Setup(repo => repo.GetBoardById(boardId))
            .ReturnsAsync(board);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.UpdateBoardCardsLeft(playerId, boardId, activePlayers));
    }

    ///

    [Fact]
    public async Task GetBoardWithBoardCards_Successful_PlayerTwoBoardCreated()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        State currentState = State.BOTH_PICKED_PLAYERS;

        Game game = new Game(playerOneId, currentState);
        game.GameID = gameId;
        game.PlayerOneID = playerOneId;
        game.PlayerTwoID = playerTwoId;

        Board boardOne = new Board(playerOneId, gameId);
        Board boardTwo = new Board(playerTwoId, gameId);
        game.Boards = new List<Board> { boardOne, boardTwo };

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act
        Board result = await _boardService.GetBoardWithBoardCards(playerTwoId, gameId);

        // Assert
        Assert.Equal(boardTwo, result);
    }

    [Fact]
    public async Task GetBoardWithBoardCards_Successful_PlayerTwoBoardNotCreated()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        State currentState = State.BOTH_PICKED_PLAYERS;

        Game game = new Game(playerOneId, currentState);
        game.GameID = gameId;
        game.PlayerOneID = playerOneId;
        game.PlayerTwoID = playerTwoId;

        Board boardOne = new Board(playerOneId, gameId);
        game.Boards = new List<Board> { boardOne };

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act
        Board result = await _boardService.GetBoardWithBoardCards(playerTwoId, gameId);

        // Assert
        Assert.Equal(boardOne, result);
    }

    [Fact]
    public async Task GetBoardWithBoardCards_GameDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerId = 12;
        int gameId = 22;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.GetBoardWithBoardCards(playerId, gameId));
    }

    [Fact]
    public async Task GetBoardWithBoardCards_PlayerOneHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;

        Game game = new Game(playerTwoId, State.BOTH_PICKED_PLAYERS);
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.GetBoardWithBoardCards(playerOneId, gameId));
    }

    [Fact]
    public async Task GetBoardWithBoardCards_PlayerTwoHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;

        Game game = new Game(playerOneId, State.BOTH_PICKED_PLAYERS);
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.GetBoardWithBoardCards(playerTwoId, gameId));
    }

    // All Other cases

    ///

    // TODO
    [Fact]
    public async Task GuessBoardCard_Successful_PlayerOneGuessedIncorrect()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        int boardId = 55;
        int cardId = 66;
        int boardCardId = 33;

        Board playerOneBoard = new Board(playerOneId, gameId);
        Board playerTwoBoard = new Board(playerTwoId, gameId);

        Game game = new Game(playerOneId, State.P1_TURN_STARTED);
        game.GameID = gameId;
        game.Boards = new List<Board> { playerOneBoard, playerTwoBoard };

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        BoardCard boardCard = new BoardCard(boardId, cardId);
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardById(boardCardId))
            .ReturnsAsync(boardCard);

        // Act

        // Assert
    }

    [Fact]
    public async Task GuessBoardCard_Successful_PlayerOneGuessedCorrect()
    {
    }

    [Fact]
    public async Task GuessBoardCard_Successful_PlayerTwoGuessedIncorrect()
    {
    }

    [Fact]
    public async Task GuessBoardCard_Successful_PlayerTwoGuessedCorrect()
    {
    }

    [Fact]
    public async Task GuessBoardCard_GameDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerOneId = 12;
        int gameId = 22;
        int boardCardId = 33;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.GuessBoardCard(playerOneId, gameId, boardCardId));
    }

    [Fact]
    public async Task GuessBoardCard_BoardCardDoesNotExist_ShouldThrow()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        int boardCardId = 33;

        Board playerOneBoard = new Board(playerOneId, gameId);
        Board playerTwoBoard = new Board(playerTwoId, gameId);

        Game game = new Game(playerOneId, State.P1_TURN_STARTED);
        game.GameID = gameId;
        game.Boards = new List<Board> { playerOneBoard, playerTwoBoard };

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardById(boardCardId))
            .ThrowsAsync(new KeyNotFoundException($"BoardCard with id {boardCardId}, does not exist!"));

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _boardService.GuessBoardCard(playerOneId, gameId, boardCardId));
    }

    [Fact]
    public async Task GuessBoardCard_PlayerOneAndTwoHasNotPermission_ShouldThrow()
    {
        // Arrange
        int playerOneId = 12;
        int playerTwoId = 12;
        int ownerPlayerId = 99;
        int gameId = 22;
        int boardId = 55;
        int cardId = 66;
        int boardCardId = 33;

        Board playerOneBoard = new Board(playerOneId, gameId);
        Board playerTwoBoard = new Board(playerTwoId, gameId);

        Game game = new Game(ownerPlayerId, State.P1_TURN_STARTED);
        game.GameID = gameId;
        game.Boards = new List<Board> { playerOneBoard, playerTwoBoard };

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        BoardCard boardCard = new BoardCard(boardId, cardId);
        _mockBoardCardRepository.Setup(repo => repo.GetBoardCardById(boardCardId))
            .ReturnsAsync(boardCard);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.GuessBoardCard(playerOneId, gameId, boardCardId));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _boardService.GuessBoardCard(playerTwoId, gameId, boardCardId));
    }

    [Fact]
    public async Task GuessBoardCard_PlayerTwoBoardNotCreated_ShouldThrow()
    {
    }
}
