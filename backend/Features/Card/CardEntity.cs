using Backend.Features.BoardCard;
using Backend.Features.Player;
using System.Text.Json.Serialization;
using Backend.Features.Shared.Common;

namespace Backend.Features.Card;

public class CardEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public int PlayerID { get; init; }

    public PlayerEntity? Player { get; init; }

    [StringLength(15, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Name { get; init; }

    [StringLength(1000, MinimumLength = 3)]
    [RegularExpression(@"^(https?://)([\w-]+(\.[\w-]+)+)(/[\w- ,./?%&=]*)?(\.(jpg|jpeg|png|gif))$", ErrorMessage = "Please enter a valid image URL.")]
    public string Url { get; init; }
    [JsonIgnore]
    public IEnumerable<BoardCardEntity>? BoardCards { get; init; }

    public CardEntity()
    {
        Name = string.Empty;
        Url = string.Empty;
    }

    public CardEntity(int playerId)
    {
        PlayerID = playerId;
        Name = string.Empty;
        Url = string.Empty;
    }
}