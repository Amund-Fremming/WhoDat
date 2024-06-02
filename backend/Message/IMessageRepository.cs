namespace MessageEntity;

public interface IMessageRepository
{
    Task<Message> GetMessageById(int messageId);

    Task<int> CreateMessage(Message message);
}
