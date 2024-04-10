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
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    public Gallery? Gallery { get; set; }
    public IEnumerable<Board>? Boards { get; set; }
    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<Game>? GamesAsPlayerOne { get; set; }
    public IEnumerable<Game>? GamesAsPlayerTwo { get; set; }

    public Player(string playerId, string username, string passwordHash, string passwordSalt)
    {
        PlayerID = playerId;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public IEnumerable<Game> GetAllGames()
    {
        if (GamesAsPlayerOne != null && GamesAsPlayerTwo != null)
            return GamesAsPlayerOne.Concat(GamesAsPlayerTwo);

        if (GamesAsPlayerOne != null)
            return GamesAsPlayerOne;

        if (GamesAsPlayerTwo != null)
            return GamesAsPlayerTwo;

        return Enumerable.Empty<Game>();
    }
}
