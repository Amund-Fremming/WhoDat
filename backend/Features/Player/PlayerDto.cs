namespace Backend.Features.Player;

public record PlayerDto(int PlayerID, string Username, string? Password, string? ImageUrl)
{
}