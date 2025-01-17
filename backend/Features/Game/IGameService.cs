using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public interface IGameService
{
    public Task<Result<int>> CreateGame(int playerId, GameState gameState);

    public Task<Result> DeleteGame(int playerId, int gameId);

    public Task<Result<GameEntity>> JoinGameById(int playerId, int gameId);

    public Task<Result> LeaveGameById(int playerId, int gameId);

    public Task<Result> UpdateGameState(int playerId, int gameId, GameState state);

    public Task<Result<int>> GetRecentGamePlayed(int playerId);

    public Task<Result<GameState>> StartGame(int playerId, int gameId);

    public Task<Result<GameState>> FinishTurn(int playerId, int gameId);
}