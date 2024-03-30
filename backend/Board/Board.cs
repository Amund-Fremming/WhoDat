namespace BoardEntity;
using System.ComponentModel.DataAnnotations;

public class Board
{
    [Key]
    public int BoardID { get; set; }
    public int GameID { get; set; }
    public int ChosenCardID { get; set; }
    public int PlayersLeft { get; set; }

    public Board() { }
}
