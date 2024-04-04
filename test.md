{
[Key]
public int GameID { get; set; }
public string PlayerOneID { get; set; }
public Player? PlayerOne { get; set; }
public string PlayerTwoID { get; set; }
public Player? PlayerTwo { get; set; }
public int CurrentPlayer { get; set; }
public State State { get; set; }

    public IEnumerable<Message> Messages { get; set; }
    public IEnumerable<Board> Boards { get; set; }

    public Game() { }

}{
[Key]
public int BoardID { get; set; }
public string PlayerID { get; set; }
public Player? Player { get; set; }
public int GameID { get; set; }
public Game? Game { get; set; }
public int ChosenCardID { get; set; }
public BoardCard? ChosenCard { get; set; }
public int PlayersLeft { get; set; }

    public IEnumerable<Message> Messages { get; set; }
    public IEnumerable<BoardCard> BoardCards { get; set; }

    public Board() { }

}{
[Key]
public int CardID { get; set; }
public int GalleryID { get; set; }
public Gallery? Gallery { get; set; }
public string? Name { get; set; }
public string? Url { get; set; }

    public IEnumerable<BoardCard> BoardCards { get; set; }

    public Card() { }

}{
[Key]
public int BoardCardID { get; set; }
public int BoardID { get; set; }
public Board? Board { get; set; }
public int CardID { get; set; }
public Card? Card { get; set; }
public bool Active { get; set; }

    public BoardCard() { }

}{
[Key]
public string PlayerID { get; set; }
public string? Username { get; set; }
public string? PasswordHash { get; set; }
public string? PasswordSalt { get; set; }

    public Gallery Gallery { get; set; }
    public IEnumerable<Board> Boards { get; set; }
    public IEnumerable<Game> Games { get; set; }
    public IEnumerable<Message> Messages { get; set; }

    public Player() { }

}{
[Key]
public int GalleryID { get; set; }
public string PlayerID { get; set; }
public Player? Player { get; set; }

    public IEnumerable<Card> Cards { get; set; }

    public Gallery() { }

}{
[Key]
public int MessageID { get; set; }
public int GameID { get; set; }
public Game Game { get; set; }
public string PlayerID { get; set; }
public Player Player { get; set; }
public string? MessageText { get; set; }

    public Message() { }

}
