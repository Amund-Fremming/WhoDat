using Backend.Features.Board;
using Backend.Features.Card;
using Backend.Features.Game;
using Backend.Features.Message;
using Backend.Features.Shared.Common;
using Backend.Features.Shared.Enums;
using System.Text.Json.Serialization;

namespace Backend.Features.Player;

public class PlayerEntity : IEntity
{
    [Key]
    public int ID { get; set; }

    public string Username { get; set; }
    public string ImageUrl { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public PlayerRole PlayerRole { get; init; }
    public IEnumerable<CardEntity>? Cards { get; init; }
    [JsonIgnore]
    public IEnumerable<BoardEntity>? Boards { get; init; }
    public IEnumerable<MessageEntity>? Messages { get; init; }
    public IEnumerable<GameEntity>? GamesAsPlayerOne { get; init; }
    public IEnumerable<GameEntity>? GamesAsPlayerTwo { get; init; }

    public PlayerEntity()
    {
        Username = string.Empty;
        PasswordHash = string.Empty;
        PasswordSalt = string.Empty;
    }

    public PlayerEntity(string username, string passwordHash, string passwordSalt, PlayerRole role, string imageUrl)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        PlayerRole = role;
        ImageUrl = imageUrl;
    }
}