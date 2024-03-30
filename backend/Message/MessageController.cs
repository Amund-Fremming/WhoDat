using Microsoft.AspNetCore.Mvc;
using MessageEntity;
using System;

namespace MessageEntity;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    public readonly IMessageService _messageService = messageService;
}