using System.ComponentModel.DataAnnotations;
using Enum;

namespace GameEntity;

public class Game
{
    [Key]
    public int GameID { get; set; }
    public string PlayerOneID { get; set; }
    public string PlayerTwoID { get; set; }
    public State State { get; set; }

    public Game() { }
}
