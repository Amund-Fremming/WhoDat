using System.ComponentModel.DataAnnotations;
using Enum;

namespace GameEntity;

public class Game
{
    [Key]
    public int GameID { get; set; }
    public int PlayerOneID { get; set; }
    public int PlayerTwoID { get; set; }
    public State State { get; set; }

    public Game() { }
}
