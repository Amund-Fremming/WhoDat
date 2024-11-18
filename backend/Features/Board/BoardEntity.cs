using Backend.Features.BoardCard;
using Backend.Features.Game;
using Backend.Features.Message;
using Backend.Features.Player;

namespace Backend.Features.Board;

public class BoardEntity
{
    [Key]
    public int BoardID { get; set; }

    public int PlayerID { get; set; }
    public PlayerEntity? Player { get; set; }
    public int GameID { get; set; }
    public GameEntity? Game { get; set; }
    public int? ChosenCardID { get; set; }
    public BoardCardEntity? ChosenCard { get; set; }
    public int PlayersLeft { get; set; }
    public IEnumerable<MessageEntity>? Messages { get; set; }
    public IEnumerable<BoardCardEntity>? BoardCards { get; set; }

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