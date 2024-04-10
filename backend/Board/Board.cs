using System.ComponentModel.DataAnnotations;
using PlayerEntity;
using GameEntity;
using BoardCardEntity;
using MessageEntity;

namespace BoardEntity;

public class Board
{
    [Key]
    public int BoardID { get; set; }
    public string PlayerID { get; set; }
    public Player? Player { get; set; }
    public int GameID { get; set; }
    public Game? Game { get; set; }
    public int ChosenCardID { get; set; }
    public BoardCard? ChosenCard { get; set; }
    public int PlayersLeft { get; set; }

    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<BoardCard>? BoardCards { get; set; }

    public Board(string playerId, int gameId, int playersLeft)
    {
        PlayerID = playerId;
        GameID = gameId;
        playersLeft = 20;
    }
}
