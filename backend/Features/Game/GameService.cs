using Backend.Features.Database;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game;

public class GameService(AppDbContext context, ILogger<IGameService> logger, IGameRepository gameRepository, IPlayerRepository playerRepository) : IGameService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<IGameService> _logger = logger;
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<Result<int>> CreateGame(int playerId, CreateGameRequest gameRequest)
    {
        try
        {
            var result = await _playerRepository.GetById(playerId);
            if (result.IsError)
                return result.Error;

            var player = result.Data;
            var game = new GameEntity(gameRequest.PlayerOneID, gameRequest.GameState)
            {
                PlayerOne = player
            };
            return await _gameRepository.Create(game);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateGame)");
            return new Error(e, "Failed to create game.");
        }
    }

    public async Task<Result> DeleteGame(int playerId, int gameId)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var validation = GameValidation.HasPermission(playerId, game);
            if (validation.IsError)
                return validation.Error;

            var gameResult = await _gameRepository.Delete(game);
            if (game.PlayerTwoID != null)
                return result.Error;

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteGame)");
            return new Error(e, "Failed to delete game");
        }
    }

    public async Task<Result<GameEntity>> JoinGameById(int playerId, int gameId)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            if (game.PlayerTwoID != null)
                throw new GameFullException($"Game with id {gameId} is full!");

            var playerResult = await _playerRepository.GetById(playerId);
            if (playerResult.IsError)
                result = playerResult.Error;

            var player = playerResult.Data;
            var joinResult = await _gameRepository.JoinGame(game, player);
            if (joinResult.IsError)
                return joinResult.Error;

            return game;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(JoinGameById)");
            return new Error(e, "Failed to join game.");
        }
    }

    public async Task<Result> LeaveGameById(int playerId, int gameId)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var validation = GameValidation.HasPermission(playerId, game);
            if (validation.IsError)
                return validation.Error;

            if (game.PlayerOneID == playerId)
                game.PlayerOneID = null;

            if (game.PlayerTwoID == playerId)
                game.PlayerTwoID = null;

            var leaveResult = await _gameRepository.LeaveGame(game);
            if (leaveResult.IsError)
                return result.Error;

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(LeaveGameById)");
            return new Error(e, "Faoled to leave game.");
        }
    }

    public async Task<Result> UpdateGameState(int playerId, int gameId, GameState state)
    {
        try
        {
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var validation = GameValidation.HasPermission(playerId, game)
                & GameValidation.CanUpdateGame(playerId, game);

            if (validation.IsError)
                return validation.Error;

            var gameResult = await _gameRepository.UpdateGameState(game, state);
            if (gameResult.IsError)
                return gameResult.Error;

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateGameState)");
            return new Error(e, "Failed to update game state.");
        }
    }

    public async Task<Result<int>> GetRecentGamePlayed(int playerId)
    {
        try
        {
            var result = await _playerRepository.GetById(playerId);
            if (result.IsError)
                return result.Error;

            return await _gameRepository.GetRecentGamePlayed(playerId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetRecentGamePlayed)");
            return new Error(e, "Failed to get recent games.");
        }
    }

    public async Task<Result<GameState>> StartGame(int playerId, int gameId)
    {
        try
        {
            var playerResult = await _playerRepository.GetById(playerId);
            var gameResult = await _gameRepository.GetById(gameId);
            if (playerResult.IsError)
                return playerResult.Error;

            if (gameResult.IsError)
                return gameResult.Error;

            var player = playerResult.Data;
            var game = gameResult.Data;
            var validation = GameValidation.HasPermission(playerId, game)
                & GameValidation.ValidState(game);

            if (validation.IsError)
                return validation.Error;

            var updateResult = await _gameRepository.UpdateGameState(game, GameState.P1_TURN_STARTED);
            if (updateResult.IsError)
                return updateResult.Error;

            return GameState.P1_TURN_STARTED;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(StartGame)");
            return new Error(e, "Failed to start game.");
        }
    }
}