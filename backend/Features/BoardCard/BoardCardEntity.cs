using Backend.Features.Board;
using Backend.Features.Card;
using Backend.Features.Shared.Common.Entity;
using System.Text.Json.Serialization;

namespace Backend.Features.BoardCard;

public class BoardCardEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public int BoardID { get; set; }
    [JsonIgnore]
    public BoardEntity? Board { get; set; }
    public int CardID { get; set; }
    [JsonIgnore]
    public CardEntity? Card { get; set; }
    public bool Active { get; set; }

    public BoardCardEntity()
    { }

    public BoardCardEntity(int boardID, int cardID)
    {
        BoardID = boardID;
        CardID = cardID;
        Active = true;
    }
}