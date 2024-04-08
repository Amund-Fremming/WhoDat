namespace MessageEntity;

public class MessageService(MessageRepository messageRepository) : IMessageService
{
    public readonly MessageRepository _messageRepository = messageRepository;

    public Task<Message> GetBoardCardById(int messageId)
    {

    }

    public Task<int> CreateMessage(Message message)
    {

    }

    public Task<bool> DeleteMessage(int messageId)
    {

    }

    public Task<bool> UpdateMessage(Message message)
    {

    }
}
