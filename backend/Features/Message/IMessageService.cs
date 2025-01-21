using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public interface IMessageService
{
    Task<Result<GameState>> CreateMessage(int playerId, int gameId, string messageText);
}