using System.ComponentModel.DataAnnotations;
using GalleryEntity;
using BoardEntity;
using GameEntity;
using MessageEntity;

namespace PlayerEntity;

public class Player
{
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
}
