using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public interface IMessageService
{
    /// <summary>
    /// Creates a new message.
    /// </summary>
    /// <param name="playerId">The player creating the message.</param>
    /// <param name="gameId">The game to create the message in.</param>
    /// <param name="messageText">The message text to be sendt.</param>
    /// <returns>The id of the new message created.</returns>
    public Task<Result<int>> CreateMessage(int playerId, int gameId, string messageText);
}