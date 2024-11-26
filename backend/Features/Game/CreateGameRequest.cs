using Backend.Features.Shared.Enums;

namespace Backend.Features.Game
{
    public record CreateGameRequest(GameState GameState)
    {
        public int PlayerOneID { get; set; }
    }
}