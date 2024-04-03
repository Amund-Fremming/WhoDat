using System.ComponentModel.DataAnnotations;
using GameEntity;
using PlayerEntity;

namespace MessageEntity;

public class Message
{
    [Key]
    public int MessageID { get; set; }
    public int GameID { get; set; }
    public Game Game { get; set; }
    public string PlayerID { get; set; }
    public Player Player { get; set; }
    public string? MessageText { get; set; }

    public Message() { }
}
