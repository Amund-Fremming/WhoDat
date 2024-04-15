namespace MessageEntity;

public class MessageService(ILogger<MessageService> logger, MessageRepository messageRepository) : IMessageService
{
    public readonly MessageRepository _messageRepository = messageRepository;
    public readonly ILogger<MessageService> _logger = logger;

    public async Task<int> CreateMessage(Message message)
    {
        try
        {
            await _messageRepository.GetMessageById(message.MessageID);

            return await _messageRepository.CreateMessage(message);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Message with id {message.MessageID}. (MessageService)");
            throw;
        }
    }

    public async Task<bool> DeleteMessage(int messageId)
    {
        try
        {
            Message message = await _messageRepository.GetMessageById(messageId);

            return await _messageRepository.DeleteMessage(message);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Message with id {messageId}. (MessageService)");
            throw;
        }
    }

    public async Task<bool> UpdateMessage(int oldMessageId, Message newMessage)
    {
        try
        {
            Message oldMessage = await _messageRepository.GetMessageById(oldMessageId);

            return await _messageRepository.UpdateMessage(oldMessage, newMessage);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating Message with id {newMessage.MessageID}. (MessageService)");
            throw;
        }
    }
}
