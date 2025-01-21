using Backend.Features.Game;
using Backend.Features.Player;
using Backend.Features.Shared.Common;

namespace Backend.Features.Message;

public class MessageEntity : IEntity
{
    [Key]
    [Range(1, 10000000)]
    public int ID { get; set; }

    [Range(1, 10000000)]
    public int GameID { get; init; }

    public GameEntity? Game { get; init; }

    [Range(1, 10000000)]
    public int PlayerID { get; init; }

    public PlayerEntity? Player { get; init; }

    [StringLength(30, MinimumLength = 5)]
    [RegularExpression(@"^[a-zA-Z0-9 ,./?:;=()""'-]*$", ErrorMessage = "Invalid characters in message.")]
    public string? MessageText { get; init; }

    public MessageEntity()
    { }

    public MessageEntity(int gameId, int playerId, string messageText)
    {
        GameID = gameId;
        PlayerID = playerId;
        MessageText = messageText;
    }
}