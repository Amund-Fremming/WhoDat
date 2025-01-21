using Backend.Features.BoardCard;
using Backend.Features.Game;
using Backend.Features.Message;
using Backend.Features.Player;
using System.Text.Json.Serialization;
using Backend.Features.Shared.Common;

namespace Backend.Features.Board;

public class BoardEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public int PlayerID { get; init; }
    public PlayerEntity? Player { get; init; }
    public int GameID { get; init; }

    [JsonIgnore]
    public GameEntity? Game { get; init; }

    public int? ChosenCardID { get; set; }
    public BoardCardEntity? ChosenCard { get; set; }
    public int PlayersLeft { get; set; }
    public IEnumerable<MessageEntity>? Messages { get; init; }
    public IEnumerable<BoardCardEntity> BoardCards { get; set; }

    public BoardEntity()
    { }

    public BoardEntity(int playerId, int gameId)
    {
        PlayerID = playerId;
        GameID = gameId;
        PlayersLeft = 20;
        ChosenCardID = null;
    }
}