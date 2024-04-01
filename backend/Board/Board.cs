using System.ComponentModel.DataAnnotations;

namespace BoardEntity;

public class Board
{
    [Key]
    public int BoardID { get; set; }
    public string PlayerID { get; set; }
    public int GameID { get; set; }
    public int ChosenCardID { get; set; }
    public int PlayersLeft { get; set; }

    public Board() { }
}
