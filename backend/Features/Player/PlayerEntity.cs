using Backend.Features.Board;
using Backend.Features.Card;
using Backend.Features.Game;
using Backend.Features.Message;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Player;

public class PlayerEntity
{
    [Key]
    public int PlayerID { get; set; }

    public string Username { get; set; }
    public string? ImageUrl { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public PlayerRole PlayerRole { get; set; }
    public IEnumerable<CardEntity>? Cards { get; set; }
    public IEnumerable<BoardEntity>? Boards { get; set; }
    public IEnumerable<MessageEntity>? Messages { get; set; }
    public IEnumerable<GameEntity>? GamesAsPlayerOne { get; set; }
    public IEnumerable<GameEntity>? GamesAsPlayerTwo { get; set; }

    public PlayerEntity(string username, string passwordHash, string passwordSalt, PlayerRole role)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        PlayerRole = role;
    }

    public IEnumerable<GameEntity> GetAllGames()
    {
        if (GamesAsPlayerOne != null && GamesAsPlayerTwo != null)
            return GamesAsPlayerOne.Concat(GamesAsPlayerTwo);

        if (GamesAsPlayerOne != null)
            return GamesAsPlayerOne;

        if (GamesAsPlayerTwo != null)
            return GamesAsPlayerTwo;

        return [];
    }
}