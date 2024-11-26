using Backend.Features.BoardCard;
using Backend.Features.Player;
using Backend.Features.Shared.Common.Entity;

namespace Backend.Features.Card;

public class CardEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public int PlayerID { get; set; }

    public PlayerEntity? Player { get; set; }

    [StringLength(15, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Name { get; set; }

    [StringLength(1000, MinimumLength = 3)]
    [RegularExpression(@"^(https?://)([\w-]+(\.[\w-]+)+)(/[\w- ,./?%&=]*)?(\.(jpg|jpeg|png|gif))$", ErrorMessage = "Please enter a valid image URL.")]
    public string Url { get; set; }

    public IEnumerable<BoardCardEntity>? BoardCards { get; set; }

    public CardEntity()
    { }

    public CardEntity(int playerId)
    {
        PlayerID = playerId;
    }
}