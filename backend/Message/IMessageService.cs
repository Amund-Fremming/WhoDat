namespace MessageEntity;

public interface IMessageService
{
    public Task<int> CreateMessage(int playerId, int gameId, Message message);
    public Task<bool> DeleteMessage(int playerId, int messageId);
}
