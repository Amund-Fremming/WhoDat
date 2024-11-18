using RaptorProject.Features.Data;

namespace MessageEntity;

public class MessageRepository(AppDbContext context, ILogger<IMessageRepository> logger) : IMessageRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IMessageRepository> _logger = logger;

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
            // TODO - more exceptions
            _logger.LogError(e, $"Error creating Message with id {message.MessageID} .(MessageRepository)");
            throw;
        }
    }
}
