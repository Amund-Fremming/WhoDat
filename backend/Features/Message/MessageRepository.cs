using Backend.Features.Database;

namespace Backend.Features.Message;

public class MessageRepository(AppDbContext context, ILogger<IMessageRepository> logger) : IMessageRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IMessageRepository> _logger = logger;

    public async Task<MessageEntity> GetMessageById(int messageId)
    {
        return await _context.Message
            .FindAsync(messageId) ?? throw new KeyNotFoundException($"Message with id {messageId}, does not exist!");
    }

    public async Task<int> CreateMessage(MessageEntity message)
    {
        try
        {
            await _context.AddAsync(message);
            return message.MessageID;
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e, $"Error creating Message with id {message.MessageID} .(MessageRepository)");
            throw;
        }
    }
}