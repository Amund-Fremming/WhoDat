namespace BoardEntity;

public class Board
{
    public int BoardID { get; set; }
    public int GameID { get; set; }
    public int ChosenCardID { get; set; }
    public int PlayersLeft { get; set; }

    public Board() { }
}
