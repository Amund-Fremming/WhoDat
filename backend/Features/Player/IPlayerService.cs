using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player
{
    public interface IPlayerService
    {
        Task<Result<PlayerDto>> Update(PlayerDto playerDto);

        Task<Result> UpdateImage(int playerId, FormFile formFile);
    }
}