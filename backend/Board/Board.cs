namespace BoardEntity;

public class Board
{
    [Key]
    public int BoardID { get; set; }
    public int PlayerID { get; set; }
    public Player? Player { get; set; }
    public int GameID { get; set; }
    public Game? Game { get; set; }
    public int? ChosenCardID { get; set; }
    public BoardCard? ChosenCard { get; set; }
    public int PlayersLeft { get; set; }
    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<BoardCard>? BoardCards { get; set; }

    public Board() { }

    public Board(int playerId, int gameId)
    {
        PlayerID = playerId;
        GameID = gameId;
        PlayersLeft = 20;
        ChosenCardID = null;
    }
}
