using System;

namespace MessageEntity;

public class MessageService(MessageRepository messageRepository) : IMessageService
{
    public readonly MessageRepository _messageRepository = messageRepository;
}