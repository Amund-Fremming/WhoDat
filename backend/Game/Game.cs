using System.ComponentModel.DataAnnotations;
using PlayerEntity;
using Enum;
using MessageEntity;
using BoardEntity;

namespace GameEntity;

public class Game
{
    [Key]
    public int GameID { get; set; }
    public int? PlayerOneID { get; set; }
    public Player? PlayerOne { get; set; }
    public int? PlayerTwoID { get; set; }
    public Player? PlayerTwo { get; set; }
    public int CurrentPlayer { get; set; }
    public State State { get; set; }

    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<Board>? Boards { get; set; }

    public Game() { }

    public Game(int playerOneId, State state)
    {
        PlayerOneID = playerOneId;
        PlayerTwoID = null;
        CurrentPlayer = 1;
        State = state;
    }
}
