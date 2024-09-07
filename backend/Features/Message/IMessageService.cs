namespace MessageEntity;

public interface IMessageService
{
    /// <summary>
    /// Creates a new message.
    /// </summary>
    /// <param name="playerId">The player creating the message.</param>
    /// <param name="gameId">The game to create the message in.</param>
    /// <param name="messageText">The message text to be sendt.</param>
    /// <returns>The id of the new message created.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the game does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the game is not in a state where this player is allowed to send a message or reply to one.</exception>
    public Task<int> CreateMessage(int playerId, int gameId, string messageText);
}
