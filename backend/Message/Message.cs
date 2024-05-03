using System.ComponentModel.DataAnnotations;
using GameEntity;
using PlayerEntity;

namespace MessageEntity;

public class Message
{
    [Key]
    [Range(1, 10000000)]
    public int MessageID { get; set; }

    [Range(1, 10000000)]
    public int GameID { get; set; }
    public Game? Game { get; set; }

    [Range(1, 10000000)]
    public int PlayerID { get; set; }
    public Player? Player { get; set; }

    [StringLength(30, MinimumLength = 5)]
    [RegularExpression(@"^[a-zA-Z0-9 ,./?:;=()""'-]*$", ErrorMessage = "Invalid characters in message.")]
    public string? MessageText { get; set; }

    public Message() { }

    public Message(int gameId, int playerId, string messageText)
    {
        GameID = gameId;
        PlayerID = playerId;
        MessageText = messageText;
    }
}
