using Backend.Features.Game;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Message;

public class MessageService(ILogger<IMessageService> logger, IMessageRepository messageRepository, IGameRepository gameRepository) : IMessageService
{
    public readonly ILogger<IMessageService> _logger = logger;
    public readonly IMessageRepository _messageRepository = messageRepository;
    public readonly IGameRepository _gameRepository = gameRepository;

    public async Task<int> CreateMessage(int playerId, int gameId, string messageText)
    {
        try
        {
            GameEntity game = await _gameRepository.GetGameById(gameId);

            bool canSendMessage = CanSendMessage(playerId, game);
            if (canSendMessage)
                return await _messageRepository.CreateMessage(new MessageEntity(playerId, gameId, messageText));

            throw new UnauthorizedAccessException($"This action is not allowed with the current game state. (MessageService)");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while creating Message for game with id {gameId}. (MessageService)");
            throw;
        }
    }

    public bool CanSendMessage(int playerId, GameEntity game)
    {
        GameState state = game.GameState;
        bool playerIsP1 = playerId == game.PlayerOneID;

        if (playerIsP1)
            return state == GameState.P1_TURN_STARTED || state == GameState.P2_WAITING_ASK_REPLY || state == GameState.P2_WAITING_GUESS_REPLY;

        if (!playerIsP1)
            return state == GameState.P2_TURN_STARTED || state == GameState.P1_WAITING_ASK_REPLY || state == GameState.P1_WAITING_GUESS_REPLY;

        return false;
    }
}