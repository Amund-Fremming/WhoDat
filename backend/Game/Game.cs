using System.ComponentModel.DataAnnotations;
using PlayerEntity;
using Enum;

namespace GameEntity;

public class Game
{
    [Key]
    public int GameID { get; set; }
    public string PlayerOneID { get; set; }
    public Player? PlayerOne { get; set; }
    public string PlayerTwoID { get; set; }
    public Player? PlayerTwo { get; set; }
    public int CurrentPlayer { get; set; }
    public State State { get; set; }

    public Game() { }
}
