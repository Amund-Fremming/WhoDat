using Data;

namespace MessageEntity;

public class MessageRepository(AppDbContext context, ILogger<MessageRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<MessageRepository> _logger = logger;

    public async Task<Message> GetMessageById(int messageId)
    {
        return await _context.Message
            .FindAsync(messageId) ?? throw new KeyNotFoundException($"Message with id {messageId}, does not exist!");
    }

    public async Task<int> CreateMessage(Message message)
    {
        try
        {
            await _context.AddAsync(message);
            return message.MessageID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Message with id {message.MessageID} .(MessageRepository)");
            return -1;
        }
    }

    public async Task<bool> DeleteMessage(Message message)
    {
        try
        {
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting Message with id {message.MessageID} .(MessageRepository)");
            return false;
        }
    }

    public async Task<bool> UpdateMessage(Message oldMessage, Message newMessage)
    {
        try
        {
            oldMessage.GameID = newMessage.GameID;
            oldMessage.Game = newMessage.Game;
            oldMessage.PlayerID = newMessage.PlayerID;
            oldMessage.Player = newMessage.Player;
            oldMessage.MessageText = newMessage.MessageText;

            _context.Message.Update(oldMessage);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating Message with id {oldMessage.MessageID} .(MessageRepository)");
            return false;
        }
    }
}
