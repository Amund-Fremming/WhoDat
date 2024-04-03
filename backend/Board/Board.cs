using System.ComponentModel.DataAnnotations;
using PlayerEntity;
using GameEntity;
using BoardCardEntity;

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

    public Board() { }
}
