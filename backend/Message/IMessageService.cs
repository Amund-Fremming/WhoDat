namespace MessageEntity;

public interface IMessageService
{
    /* Basic CRUD */
    public Task<Message> GetBoardCardById(int messageId);
    public Task<int> CreateMessage(Message message);
    public Task DeleteMessage(int messageId);
    public Task UpdateMessage(Message message);

    /* Other */
    public Task SendMessage(Message message); // Do i need this?
}
