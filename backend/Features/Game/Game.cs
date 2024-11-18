using RaptorProject.Features.Shared.Enums;

namespace GameEntity;

public class Game
{
    [Key]
    [Range(1, 10000000)]
    public int GameID { get; set; }

    [Range(1, 10000000)]
    public int? PlayerOneID { get; set; }
    public PlayerEntity.Player? PlayerOne { get; set; }

    [Range(1, 10000000)]
    public int? PlayerTwoID { get; set; }
    public PlayerEntity.Player? PlayerTwo { get; set; }

    public State State { get; set; }

    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<Board>? Boards { get; set; }

    public Game() { }

    public Game(int playerOneId, State state)
    {
        PlayerOneID = playerOneId;
        PlayerTwoID = null;
        State = state;
    }
}
