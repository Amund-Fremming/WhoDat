using System;

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

    public Task DeleteMessage(int messageId)
    {

    }

    public Task UpdateMessage(Message message)
    {

    }

    public Task SendMessage(Message message)
    {

    }
}
