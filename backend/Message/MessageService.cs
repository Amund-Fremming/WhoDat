using GameEntity;
using Enum;

namespace MessageEntity;

public class MessageService(ILogger<MessageService> logger, MessageRepository messageRepository, GameRepository gameRepository) : IMessageService
{
    public readonly ILogger<MessageService> _logger = logger;
    public readonly MessageRepository _messageRepository = messageRepository;
    public readonly GameRepository _gameRepository = gameRepository;

    public async Task<int> CreateMessage(int playerId, int gameId, string messageText)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);
            int? currentPlayerId = game.CurrentPlayer == 1 ? game.PlayerOneID : game.PlayerTwoID;

            bool canSendMessage = CanSendMessage(playerId, currentPlayerId, game.State);
            if (canSendMessage)
                return await _messageRepository.CreateMessage(new Message(playerId, gameId, messageText));

            return -1;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Message for game with id {gameId}. (MessageService)");
            throw;
        }
    }

    public bool CanSendMessage(int playerId, int? currentPlayerId, State currentState)
    {
        if (currentPlayerId == null)
            throw new Exception($"Current player does is null (MessageService)");

        bool isCurrentPlayer = currentPlayerId == playerId;

        if (isCurrentPlayer)
            return (currentState == State.ASKING || currentState == State.GUESSING);

        if (!isCurrentPlayer)
            return (currentState == State.WAITING_ASK_REPLY || currentState == State.WAITING_GUESS_REPLY);

        return false;
    }
}
