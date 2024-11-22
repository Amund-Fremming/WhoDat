using Backend.Features.Database;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Message;

public class MessageRepository(AppDbContext context, ILogger<IMessageRepository> logger) : IMessageRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IMessageRepository> _logger = logger;

    public async Task<Result<MessageEntity>> GetMessageById(int messageId)
    {
        try
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message == null)
                return new Error(new KeyNotFoundException($"Message with id {messageId}, does not exist!"), "Message does not exist.");

            return message;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetMessageById)");
            return new Error(e, "Failed getting message.");
        }
    }

    public async Task<Result<int>> CreateMessage(MessageEntity message)
    {
        try
        {
            await _context.AddAsync(message);
            return message.MessageID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateMessage)");
            return new Error(e, "Failed to create message.");
        }
    }
}