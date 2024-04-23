namespace MessageEntity;

public interface IMessageService
{
    public Task<int> CreateMessage(int playerId, int gameId, Message message);
}
