namespace MessageEntityTest;

public class MessageServiceTest
{
    private readonly Mock<ILogger<MessageService>> _mockLogger;
    private readonly Mock<IMessageRepository> _mockMessageRepository;
    private readonly Mock<IGameRepository> _mockGameRepository;
    private readonly IMessageService _messageService;

    public MessageServiceTest()
    {
        _mockLogger = new Mock<ILogger<MessageService>>();
        _mockMessageRepository = new Mock<IMessageRepository>();
        _mockGameRepository = new Mock<IGameRepository>();

        _messageService = new MessageService(
                _mockLogger.Object,
                _mockMessageRepository.Object,
                _mockGameRepository.Object
        );
    }

    [Fact]
    public async Task CreateMessage_Successful_CanSendMessage()
    {
        int playerId = 12;
        int gameId = 22;
        int messageId = 33;
        string messageText = "Hello World!";
        State currentState = State.P1_TURN_STARTED;

        Game game = new Game(playerId, currentState);
        game.GameID = gameId;
        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        _mockMessageRepository.Setup(repo => repo.CreateMessage(It.IsAny<Message>()))
            .ReturnsAsync(messageId);

        int result = await _messageService.CreateMessage(playerId, gameId, messageText);

        Assert.Equal(messageId, result);
    }

    [Fact]
    public async Task CreateMessage_GameDoesNotExist_ShouldThrow()
    {
        int playerId = 12;
        int gameId = 22;
        string messageText = "Hello World!";

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ThrowsAsync(new KeyNotFoundException($"Game with id {gameId}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _messageService.CreateMessage(playerId, gameId, messageText));
    }

    [Fact]
    public async Task CreateMessage_IsPlayerOneCantSendMessage_ShouldThrow()
    {
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        string messageText = "Hello World!";
        State currentState = State.P2_TURN_STARTED;

        Game game = new Game(playerOneId, currentState);
        game.GameID = gameId;
        game.PlayerOneID = playerOneId;
        game.PlayerTwoID = playerTwoId;
        game.State = currentState;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _messageService.CreateMessage(playerOneId, gameId, messageText));
    }


    [Fact]
    public async Task CreateMessage_IsPlayerTwoCantSendMessage_ShouldThrow()
    {
        int playerOneId = 12;
        int playerTwoId = 13;
        int gameId = 22;
        string messageText = "Hello World!";
        State currentState = State.P1_TURN_STARTED;

        Game game = new Game(playerOneId, currentState);
        game.GameID = gameId;
        game.PlayerOneID = playerOneId;
        game.PlayerTwoID = playerTwoId;
        game.State = currentState;

        _mockGameRepository.Setup(repo => repo.GetGameById(gameId))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _messageService.CreateMessage(playerTwoId, gameId, messageText));
    }
}
