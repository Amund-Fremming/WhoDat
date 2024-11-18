using Backend.Features.Game;
using Backend.Features.Player;

namespace Backend.Features.Message;

public class MessageEntity
{
    [Key]
    [Range(1, 10000000)]
    public int MessageID { get; set; }

    [Range(1, 10000000)]
    public int GameID { get; set; }

    public GameEntity? Game { get; set; }

    [Range(1, 10000000)]
    public int PlayerID { get; set; }

    public PlayerEntity? Player { get; set; }

    [StringLength(30, MinimumLength = 5)]
    [RegularExpression(@"^[a-zA-Z0-9 ,./?:;=()""'-]*$", ErrorMessage = "Invalid characters in message.")]
    public string? MessageText { get; set; }

    public MessageEntity()
    { }

    public MessageEntity(int gameId, int playerId, string messageText)
    {
        GameID = gameId;
        PlayerID = playerId;
        MessageText = messageText;
    }
}