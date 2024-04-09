namespace MessageEntity;

public interface IMessageService
{
    /* Basic CRUD */
    public Task<int> CreateMessage(Message message);
    public Task<bool> DeleteMessage(int messageId);
    public Task<bool> UpdateMessage(Message newMessage);
}
