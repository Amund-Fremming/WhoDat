using Backend.Features.Board;
using Backend.Features.Card;
using System.Text.Json.Serialization;
using Backend.Features.Shared.Common;

namespace Backend.Features.BoardCard;

public class BoardCardEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public int BoardID { get; init; }

    [JsonIgnore]
    public BoardEntity Board { get; init; }

    public int CardID { get; init; }
    public CardEntity? Card { get; init; }
    public bool Active { get; set; }

    public BoardCardEntity()
    { }

    public BoardCardEntity(int boardId, int cardId)
    {
        BoardID = boardId;
        CardID = cardId;
        Active = true;
    }
}