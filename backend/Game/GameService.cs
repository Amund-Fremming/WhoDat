using Enum;
using PlayerEntity;
using Data;

namespace GameEntity;

public class GameService(AppDbContext context, ILogger<GameService> logger, GameRepository gameRepository, PlayerRepository playerRepository) : IGameService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GameService> _logger = logger;
    public readonly GameRepository _gameRepository = gameRepository;
    public readonly PlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateGame(Game game, int playerId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Player player = await _playerRepository.GetPlayerById(playerId);
                int gameId = await _gameRepository.CreateGame(game, player);

                await transaction.CommitAsync();
                return gameId;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while creating Game with id {game.GameID}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> DeleteGame(int gameId)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.DeleteGame(game);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while deleting Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<bool> JoinGameById(int gameId, int playerId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                if (game.PlayerTwoID != -1) return false;

                Player player = await _playerRepository.GetPlayerById(playerId);
                bool joinedGame = await _gameRepository.JoinGame(game, player);

                await transaction.CommitAsync();
                return joinedGame;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while joining Game with id {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> LeaveGameById(int gameId, int playerNumber)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);

                if (playerNumber == 1)
                {
                    game.PlayerOneID = -1;
                    game.PlayerOne = null;
                    game.State = State.P2_WON;
                }

                if (playerNumber == 2)
                {
                    game.PlayerTwoID = -1;
                    game.PlayerTwo = null;
                    game.State = State.P1_WON;
                }

                bool leftGame = await _gameRepository.LeaveGame(game);

                await transaction.CommitAsync();
                return leftGame;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while leaving Game with id {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> UpdateGameState(int gameId, State state)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);

            return await _gameRepository.UpdateGameState(game, state);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating state in Game with id {gameId}. (GameService)");
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
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while updating current player turn in Game with id {gameId}. (GameService)");
            throw;
        }
    }
}
