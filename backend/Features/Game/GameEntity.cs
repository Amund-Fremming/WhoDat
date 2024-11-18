using Backend.Features.Board;
using Backend.Features.Message;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Game;

public class GameEntity
{
    [Key]
    [Range(1, 10000000)]
    public int GameID { get; set; }

    [Range(1, 10000000)]
    public int? PlayerOneID { get; set; }

    public PlayerEntity? PlayerOne { get; set; }

    [Range(1, 10000000)]
    public int? PlayerTwoID { get; set; }

    public PlayerEntity? PlayerTwo { get; set; }

    public GameState GameState { get; set; }

    public IEnumerable<MessageEntity>? Messages { get; set; }
    public IEnumerable<BoardEntity>? Boards { get; set; }

    public GameEntity()
    { }

    public GameEntity(int playerOneId, GameState gameState)
    {
        PlayerOneID = playerOneId;
        PlayerTwoID = null;
        GameState = gameState;
    }
}