using System;
using Data;

namespace MessageEntity;

public class MessageRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}