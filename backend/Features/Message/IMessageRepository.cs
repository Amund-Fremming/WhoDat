using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public interface IMessageRepository
{
    /// <summary>
    /// Get a Message corresponding to the given id.
    /// </summary>
    /// <param name="messageId">The id for the Message.</param>
    /// <returns>The Message asked for.</returns>
    Task<Result<MessageEntity>> GetMessageById(int messageId);

    /// <summary>
    /// Stores a new Message to the database.
    /// </summary>
    /// <param name="message">The Message to be stored.</param>
    Task<Result<int>> CreateMessage(MessageEntity message);
}