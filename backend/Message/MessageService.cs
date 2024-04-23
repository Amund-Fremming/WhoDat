using GameEntity;
using Enum;

namespace MessageEntity;

public class MessageService(ILogger<MessageService> logger, MessageRepository messageRepository, GameRepository gameRepository) : IMessageService
{
    public readonly ILogger<MessageService> _logger = logger;
    public readonly MessageRepository _messageRepository = messageRepository;
    public readonly GameRepository _gameRepository = gameRepository;

    public async Task<int> CreateMessage(int playerId, int gameId, Message message)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);
            if (game.PlayerOneID == -1 || game.PlayerTwoID == -1)
                throw new Exception($"Not enough player for sending messages!");

            int currentPlayerId = game.CurrentPlayer == 1 ? game.PlayerOneID : game.PlayerTwoID;
            bool canSendMessage = CanSendMessage(playerId, currentPlayerId, game.State);

            if (canSendMessage)
                return await _messageRepository.CreateMessage(message);

            return -1;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Message with id {message.MessageID}. (MessageService)");
            throw;
        }
    }

    public bool CanSendMessage(int playerId, int currentPlayerId, State currentState)
    {
        bool isCurrentPlayer = currentPlayerId == playerId;

        if (isCurrentPlayer)
            return (currentState == State.ASKING || currentState == State.GUESSING);

        if (!isCurrentPlayer)
            return (currentState == State.WAITING_ASK_REPLY || currentState == State.WAITING_GUESS_REPLY);

        return false;
    }
}
