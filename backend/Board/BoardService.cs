using BoardCardEntity;
using Data;
using GameEntity;
using Enum;

namespace BoardEntity;

public class BoardService(ILogger<BoardService> logger, AppDbContext context, BoardRepository boardRepository, BoardCardRepository boardCardRepository, GameRepository gameRepository) : IBoardService
{
    public readonly ILogger<BoardService> _logger = logger;
    public readonly AppDbContext _context = context;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly BoardCardRepository _boardCardRepository = boardCardRepository;
    public readonly GameRepository _gameRepository = gameRepository;

    public async Task<int> CreateBoard(int playerId, int gameId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = new Board(playerId, gameId);
                board.PlayerID = playerId;
                int boardId = await _boardRepository.CreateBoard(board);

                await transaction.CommitAsync();
                return boardId;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while creating Board. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task DeleteBoard(int playerId, int boardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                await _boardRepository.DeleteBoard(board);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while deleting Board with id {boardId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task ChooseCard(int playerId, int boardId, int boardCardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);
                BoardCard boardCard = await _boardCardRepository.GetBoardCardById(boardCardId);

                await _boardRepository.ChooseCard(board, boardCard);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error chosing a card on Board with id {boardId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task UpdatePlayersLeft(int playerId, int boardId, int activePlayers)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            await _boardRepository.UpdatePlayersLeft(board, activePlayers);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error updating card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public async Task<Board> GetBoardWithBoardCards(int playerId, int gameId)
    {
        try
        {
            // TODO - if playerTwoBoard is not created, create a duplicate from player one
            Game game = await _gameRepository.GetGameById(gameId);
            PlayerHasGamePermission(playerId, game);

            Board playerOneBoard = game.Boards!.ElementAt(0);
            Board playerTwoBoard = game.Boards!.ElementAt(1);

            if (playerOneBoard.PlayerID == playerId)
                return playerOneBoard;

            if (playerTwoBoard.PlayerID == playerId)
                return playerTwoBoard;

            return null!;
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error fetching Board from game with id {gameId}. (BoardService)");
            throw;
        }
    }

    public async Task<State> GuessBoardCard(int playerId, int gameId, int boardCardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                Board otherPlayersBoard;

                if (game.Boards!.ElementAt(0).PlayerID == playerId)
                    otherPlayersBoard = game.Boards!.ElementAt(1);
                else
                    otherPlayersBoard = game.Boards!.ElementAt(0);

                BoardCard guessedCard = await _boardCardRepository.GetBoardCardById(boardCardId);
                PlayerHasGamePermission(playerId, game);

                if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                    game.State = State.P1_WON;
                if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                    game.State = State.P2_WON;
                if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                    game.State = State.P1_TURN_STARTED;
                if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                    game.State = State.P2_TURN_STARTED;

                await _gameRepository.UpdateGame(game);
                await transaction.CommitAsync();
                return game.State;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error taking in guess on game wiht id {gameId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public void PlayerHasGamePermission(int playerId, Game game)
    {
        if (game.PlayerOneID != playerId || game.PlayerTwoID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }


    public void PlayerHasPermission(int playerId, Board board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }
}
