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

    [Fact]
    public async Task CreateGame_Successful_ReturnsId()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;
        game.PlayerTwoID = null;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.CreateGame(game, player))
            .ReturnsAsync(gameId);

        int result = await _gameService.CreateGame(playerId, game);

        Assert.Equal(gameId, result);
    }

    [Fact]
    public async Task CreateGame_PlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.CreateGame(playerId, new Game()));
    }

    [Fact]
    public async Task DeleteGame_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockGameRepository.Setup(repo => repo.DeleteGame(game))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _gameService.DeleteGame(playerId, gameId);

        _mockGameRepository.Verify(repo => repo.DeleteGame(game));
    }

    [Fact]
    public async Task DeleteGame_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.DeleteGame(playerId, gameId));
    }

    [Fact]
    public async Task DeleteGame_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 13;
        int gameId = 2;

        Player owner = new Player("OwnerUsername", "PasswordHash", "PasswordSalt", Role.USER);
        owner.PlayerID = ownerPlayerId;

        Game game = new Game(ownerPlayerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _gameService.DeleteGame(playerId, gameId));
    }

    [Fact]
    public async Task JoinGameById_Successful_ReturnsGame()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(13, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.JoinGame(game, player))
            .Returns(Task.CompletedTask);

        Game result = await _gameService.JoinGameById(playerId, gameId);

        Assert.Equal(game, result);
    }

    [Fact]
    public async Task JoinGameById_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.JoinGameById(playerId, gameId));
    }

    [Fact]
    public async Task JoinGameById_GameIsFull_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Game game = new Game(13, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;
        game.PlayerTwoID = 14;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<GameFullException>(() => _gameService.JoinGameById(playerId, gameId));
    }

    [Fact]
    public async Task JoinGameById_PlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Game game = new Game(13, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.JoinGameById(playerId, gameId));
    }

    [Fact]
    public async Task LeaveGameById_Successful()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.LeaveGame(game))
            .Returns(Task.CompletedTask);

        await _gameService.LeaveGameById(playerId, gameId);

        _mockGameRepository.Verify(repo => repo.LeaveGame(game));
    }

    [Fact]
    public async Task LeaveGameById_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.LeaveGameById(playerId, gameId));
    }

    [Fact]
    public async Task LeaveGameById_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 13;
        int gameId = 2;

        Player owner = new Player("OwnerUsername", "PasswordHash", "PasswordSalt", Role.USER);
        owner.PlayerID = ownerPlayerId;

        Game game = new Game(ownerPlayerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _gameService.LeaveGameById(playerId, gameId));
    }

    [Fact]
    public async Task UpdateGameState_Successful_PlayerHasPermission()
    {
        int playerId = 12;
        int gameId = 2;
        State newState = State.P1_TURN_STARTED;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;
        game.PlayerOneID = playerId;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.UpdateGameState(game, newState))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _gameService.UpdateGameState(playerId, gameId, newState);

        _mockGameRepository.Verify(repo => repo.UpdateGameState(game, newState));
    }

    [Fact]
    public async Task UpdateGameState_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;
        State newState = State.P1_TURN_STARTED;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.UpdateGameState(playerId, gameId, newState));
    }

    [Fact]
    public async Task GetRecentGamePlayed_Successful()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetRecentGamePlayed(playerId))
            .ReturnsAsync(gameId);

        int result = await _gameService.GetRecentGamePlayed(playerId);

        Assert.Equal(gameId, result);
    }

    [Fact]
    public async Task GetRecentGamePlayed_PlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.GetRecentGamePlayed(playerId));
    }

    [Fact]
    public async Task StartGame_Successful_ReturnsNewState()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;
        game.PlayerTwoID = 13;
        game.Boards = new List<Board>
        {
            new Board { ChosenCardID = 1 },
            new Board { ChosenCardID = 2 }
        };

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockGameRepository.Setup(repo => repo.UpdateGameState(game, State.P1_TURN_STARTED))
            .Returns(Task.CompletedTask);

        State result = await _gameService.StartGame(playerId, gameId);

        Assert.Equal(State.P1_TURN_STARTED, result);
    }

    [Fact]
    public async Task StartGame_PlayerDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {playerId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_PlayerHasNotPermission_ShouldThrow()
    {
        int playerId = 12;
        int ownerPlayerId = 13;
        int gameId = 2;

        Player owner = new Player("OwnerUsername", "PasswordHash", "PasswordSalt", Role.USER);
        owner.PlayerID = ownerPlayerId;

        Game game = new Game(ownerPlayerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(owner);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_StateIsNotCorrect_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_CHOSING_CARDS);
        game.GameID = gameId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_NotEnoughPlayers_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_PlayerOneNotCreatedBoard_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;
        game.PlayerTwoID = 13;
        game.Boards = new List<Board>
        {
            new Board { ChosenCardID = null },
            new Board { ChosenCardID = 2 }
        };

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_PlayerTwoNotCreatedBoard_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;
        game.PlayerTwoID = 13;
        game.Boards = new List<Board>
        {
            new Board { ChosenCardID = 1 },
            new Board { ChosenCardID = null }
        };

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _gameService.StartGame(playerId, gameId));
    }

    [Fact]
    public async Task StartGame_PlayerOneNotChoosenPlayingCard_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 2;

        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        player.PlayerID = playerId;

        Game game = new Game(playerId, State.BOTH_PICKED_PLAYERS);
        game.GameID = gameId;
        game.PlayerTwoID = 13;
        game.Boards = new List<Board>
        {
            new Board { ChosenCardID = null },
            new Board { ChosenCardID = 2 }
        };

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(playerId))
            .ReturnsAsync(player);

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _gameService.StartGame(playerId, gameId));
    }
}

