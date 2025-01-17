using Backend.Features.Game;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public class MessageService(ILogger<IMessageService> logger, IMessageRepository messageRepository, IGameRepository gameRepository) : IMessageService
{
    private readonly ILogger<IMessageService> _logger = logger;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<GameState>> CreateMessage(int playerId, int gameId, string messageText)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var canSendMessage = CanSendMessage(playerId, game);
            if (!canSendMessage)
                return new Error(new InvalidOperationException("Player cannot send message in current context"), "Cannot send message in current state.");

            var messageResult = await _messageRepository.Create(new MessageEntity(gameId, playerId, messageText));
            if (messageResult.IsError)
                return messageResult.Error;

            game.GameState = GetNextGameState(playerId == game.PlayerOneID, game.GameState);

            var stateResult = await _gameRepository.UpdateGame(game);
            if (stateResult.IsError)
                return stateResult.Error;

            return game.GameState;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateMessage)");
            return new Error(e, "Failed to create message.");
        }
    }

    private static GameState GetNextGameState(bool isPlayerOne, GameState gameState)
    {
        return gameState switch
        {
            GameState.P1_TURN_STARTED when isPlayerOne => GameState.P1_WAITING_ASK_REPLY,
            GameState.P2_ASK_REPLIED when isPlayerOne => GameState.P1_TURN_FINISHED,
            GameState.P2_TURN_STARTED when isPlayerOne => GameState.P2_WAITING_ASK_REPLY,
            GameState.P1_ASK_REPLIED when isPlayerOne => GameState.P2_TURN_FINISHED,
            _ => gameState
        };
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