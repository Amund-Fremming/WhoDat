namespace GameEntity;

public class GameService(AppDbContext context, ILogger<IGameService> logger, IGameRepository gameRepository, IPlayerRepository playerRepository) : IGameService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IGameService> _logger = logger;
    public readonly IGameRepository _gameRepository = gameRepository;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateGame(int playerId, Game game)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Player player = await _playerRepository.GetPlayerById(playerId);
                game.PlayerTwoID = null;
                int gameId = await _gameRepository.CreateGame(game, player);

                await transaction.CommitAsync();
                return gameId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while creating Game with id {game.GameID}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task DeleteGame(int playerId, int gameId)
    {
        try
        {
            Game game = await _gameRepository.GetGameById(gameId);
            PlayerHasPermission(playerId, game);

            await _gameRepository.DeleteGame(game);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting Game with id {gameId}. (GameService)");
            throw;
        }
    }

    public async Task<Game> JoinGameById(int playerId, int gameId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);

                if (game.PlayerTwoID != null)
                    throw new GameFullException($"Game with id {gameId} is full!");

                Player player = await _playerRepository.GetPlayerById(playerId);
                await _gameRepository.JoinGame(game, player);

                await transaction.CommitAsync();
                return game;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while joining Game with id {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task LeaveGameById(int playerId, int gameId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                PlayerHasPermission(playerId, game);

                if (game.PlayerOneID == playerId)
                    game.PlayerOneID = null;

                if (game.PlayerTwoID == playerId)
                    game.PlayerTwoID = null;

                await _gameRepository.LeaveGame(game);

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while leaving Game with id {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task UpdateGameState(int playerId, int gameId, State state)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                PlayerHasPermission(playerId, game);
                PlayerCanUpdateGame(playerId, game);

                await _gameRepository.UpdateGameState(game, state);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while updating state in Game with id {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<int> GetRecentGamePlayed(int playerId)
    {
        try
        {
            await _playerRepository.GetPlayerById(playerId);

            return await _gameRepository.GetRecentGamePlayed(playerId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting players recent Game with PlayerID {playerId}. (GameService)");
            throw;
        }
    }

    public async Task<State> StartGame(int playerId, int gameId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Player player = await _playerRepository.GetPlayerById(playerId);
                Game game = await _gameRepository.GetGameById(gameId);
                PlayerHasPermission(playerId, game);

                if (game.State != State.BOTH_PICKED_PLAYERS)
                    throw new ArgumentOutOfRangeException("Game cannot start at this state!");

                if (game.PlayerOneID == null || game.PlayerTwoID == null)
                    throw new ArgumentOutOfRangeException("Game cannot start, missing players!");

                Board playerOneBoard = game.Boards!.ElementAt(0);
                Board playerTwoBoard = game.Boards!.ElementAt(1) ??
                    throw new ArgumentOutOfRangeException("Game cannot start, player(s) have not created their board!");

                if (playerOneBoard.ChosenCardID == null || playerTwoBoard.ChosenCardID == null)
                    throw new ArgumentOutOfRangeException("Game cannot start, player(s) have not choosen boardcard!");

                await _gameRepository.UpdateGameState(game, State.P1_TURN_STARTED);
                return State.P1_TURN_STARTED;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while starting Game with ID {gameId}. (GameService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public void PlayerHasPermission(int playerId, Game game)
    {
        if (playerId != game.PlayerOneID && playerId != game.PlayerTwoID)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (GameService)");
        }
    }

    public void PlayerCanUpdateGame(int playerId, Game game)
    {
        bool isPlayerOne = game.PlayerOneID == playerId;

        if ((isPlayerOne && (game.State != State.P1_ASK_REPLIED && game.State != State.P1_TURN_STARTED && game.State != State.P1_GUESS_REPLIED)) ||
            (!isPlayerOne && (game.State != State.P2_ASK_REPLIED && game.State != State.P2_TURN_STARTED && game.State != State.P2_GUESS_REPLIED)))
        {
            _logger.LogInformation($"Player with id {playerId} tried to update the state when not allowed.");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission to update the state (GameService)");
        }
    }
}

