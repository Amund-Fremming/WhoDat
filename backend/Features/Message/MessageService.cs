using Backend.Features.Game;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public class MessageService(ILogger<IMessageService> logger, IMessageRepository messageRepository, IGameRepository gameRepository) : IMessageService
{
    public readonly ILogger<IMessageService> _logger = logger;
    public readonly IMessageRepository _messageRepository = messageRepository;
    public readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<int>> CreateMessage(int playerId, int gameId, string messageText)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            bool canSendMessage = CanSendMessage(playerId, game);
            if (!canSendMessage)
                return new Error(new InvalidOperationException("Player cannot send message in current context"), "Cannot send message in current state.");

            return await _messageRepository.Create(new MessageEntity(playerId, gameId, messageText));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateMessage)");
            return new Error(e, "Failed to create message.");
        }
    }

    private static bool CanSendMessage(int playerId, GameEntity game)
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