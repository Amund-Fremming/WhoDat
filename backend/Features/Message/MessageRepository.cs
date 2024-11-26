using Backend.Features.Database;
using Backend.Features.Shared.Common.Repository;

namespace Backend.Features.Message;

public class MessageRepository(AppDbContext context, ILogger<MessageRepository> logger)
    : RepositoryBase<MessageEntity, MessageRepository>(logger, context), IMessageRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<MessageRepository> _logger = logger;
}