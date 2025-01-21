using Backend.Features.Shared.Common;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player;

public interface IPlayerRepository : IRepository<PlayerEntity>
{
    Task<Result<PlayerEntity>> GetPlayerByUsername(string username);

    Task<Result<IEnumerable<PlayerDto>>> GetAllPlayers();

    Task<Result> UsernameExist(string username);

    Task<Result> Update(PlayerEntity player);
}