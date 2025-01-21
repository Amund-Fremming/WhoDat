using Backend.Features.Player;
using Backend.Features.Shared.Common;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public interface IGameRepository : IRepository<GameEntity>
{
    Task<Result<GameEntity>> GetGameWithBoards(int gameId);
    
    Task<Result> JoinGame(GameEntity game, PlayerEntity player);
    
    Task<Result> LeaveGame(GameEntity game);
    
    Task<Result> UpdateGameState(GameEntity game, GameState state);
    
    Result<int> GetRecentGamePlayed(int playerId);
    
    Task<Result> UpdateGame(GameEntity game);
}