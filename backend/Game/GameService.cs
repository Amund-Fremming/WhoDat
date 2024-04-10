using Enum;
using PlayerEntity;

namespace GameEntity;

public class GameService(ILogger<GameService> logger, GameRepository gameRepository, PlayerRepository playerRepository) : IGameService
{
    public readonly ILogger<GameService> _logger = logger;
    public readonly GameRepository _gameRepository = gameRepository;
    public readonly PlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateGame(Game game, Player player)
    {
        try
        {
            await _gameRepository.GetGameById(game.GameID);
            await _playerRepository.GetPlayerById(player.PlayerID);

            return await _gameRepository.CreateGame(game, player);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while creating Game with id {game.GameID}. (GameService)");
            throw;
        }
    }

    public async Task<bool> DeleteGame(int gameId)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.DeleteGame(game);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while deleting Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<bool> JoinGameById(int gameId, string playerId)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            if (!String.IsNullOrEmpty(game.PlayerOneID) || !String.IsNullOrEmpty(game.PlayerTwoID))
                return false;


            Player player = await _playerRepository.GetPlayerById(playerId);

            return await _gameRepository.JoinGame(game, player);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while joining Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<bool> LeaveGameById(int gameId, int playerNumber)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.LeaveGame(game, playerNumber);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while leaving Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<bool> UpdateGameState(int gameId, State state)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.UpdateGameState(game, state);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while updating state in Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<bool> UpdateCurrentPlayerTurn(int gameId, int playerNumber)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.UpdateCurrentPlayerTurn(game, playerNumber);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while updating current player turn in Game with id {gameId}. (GameService)");
            throw;
        }
    }
}
