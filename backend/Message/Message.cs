using System.ComponentModel.DataAnnotations;

namespace MessageEntity;

public class Message
{
    [Key]
    public int MessageID { get; set; }
    public int GameID { get; set; }
    public string? MessageString { get; set; }

    public Message() { }
}
