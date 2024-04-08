using Data;

namespace MessageEntity;

public class MessageRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;

    public async Task<Message> GetBoardCardById(int messageId)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }

    public async Task<int> CreateMessage(Message message)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }

    public async Task DeleteMessage(int messageId)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }

    public async Task UpdateMessage(Message message)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }

    public async Task SendMessage(Message message)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }

}
