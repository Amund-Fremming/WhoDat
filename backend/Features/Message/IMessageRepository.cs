namespace Backend.Features.Message;

public interface IMessageRepository
{
    /// <summary>
    /// Get a Message corresponding to the given id.
    /// </summary>
    /// <param name="messageId">The id for the Message.</param>
    /// <returns>The Message asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Message does not exist.</exception>
    Task<MessageEntity> GetMessageById(int messageId);

    /// <summary>
    /// Stores a new Message to the database.
    /// </summary>
    /// <param name="message">The Message to be stored.</param>
    Task<int> CreateMessage(MessageEntity message);
}