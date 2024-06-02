using GameEntity;
using Enum;

namespace MessageEntity;

public class MessageService(ILogger<IMessageService> logger, IMessageRepository messageRepository, IGameRepository gameRepository) : IMessageService
{
    public readonly ILogger<IMessageService> _logger = logger;
    public readonly IMessageRepository _messageRepository = messageRepository;
    public readonly IGameRepository _gameRepository = gameRepository;

    public async Task<int> CreateMessage(int playerId, int gameId, string messageText)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            bool canSendMessage = CanSendMessage(playerId, game);
            if (canSendMessage)
                return await _messageRepository.CreateMessage(new Message(playerId, gameId, messageText));

            throw new UnauthorizedAccessException($"This action is not allowed with the current game state. (MessageService)");
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Message for game with id {gameId}. (MessageService)");
            throw;
        }
    }

    public bool CanSendMessage(int playerId, Game game)
    {
        State state = game.State;
        bool playerIsP1 = playerId == game.PlayerOneID;

        if (playerIsP1)
            return (state == State.P1_TURN_STARTED || state == State.P2_WAITING_ASK_REPLY || state == State.P2_WAITING_GUESS_REPLY);


        if (!playerIsP1)
            return (state == State.P2_TURN_STARTED || state == State.P1_WAITING_ASK_REPLY || state == State.P1_WAITING_GUESS_REPLY);

        return false;
    }
}
