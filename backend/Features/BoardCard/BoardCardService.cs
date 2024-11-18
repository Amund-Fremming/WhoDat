using Backend.Features.Board;
using Backend.Features.Card;
using Backend.Features.Database;
using Backend.Features.Game;
using Backend.Features.Shared.Enums;

namespace Backend.Features.BoardCard;

public class BoardCardService(AppDbContext context, ILogger<IBoardCardService> logger,
        IBoardCardRepository boardcardRepository, IBoardRepository boardRepository, ICardRepository cardRepository, IGameRepository gameRepository) : IBoardCardService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IBoardCardService> _logger = logger;
    public readonly IBoardCardRepository _boardcardRepository = boardcardRepository;
    public readonly IBoardRepository _boardRepository = boardRepository;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IGameRepository _gameRepository = gameRepository;

    public async Task<GameState> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                GameEntity game = await _gameRepository.GetGameById(gameId);

                PlayerHasGamePermission(playerId, game);
                ValidatePlayerPermissions(playerId, game, cardIds);

                int boardId = game.Boards!.ElementAt(0).BoardID;

                if (game.Boards!.ElementAt(0) == null)
                {
                    BoardEntity board = new(playerId, gameId);
                    boardId = await _boardRepository.CreateBoard(board);
                    _logger.LogInformation("Board not created, creating board...");
                }

                if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS)
                    cardIds = cardIds.Take(20);
                else
                    cardIds = cardIds.Take(10);

                IEnumerable<BoardCardEntity> newBoardCards = cardIds.Select(cardId => new BoardCardEntity(boardId, cardId)).ToList();

                bool isPlayerOne = game.PlayerOneID == playerId;
                if (game.GameState == GameState.P1_CHOOSING && !isPlayerOne || game.GameState == GameState.P2_CHOOSING && isPlayerOne)
                    throw new ArgumentException("Player cannot create more BoardCards!");
                else if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS || game.GameState == GameState.P1_CHOOSING && isPlayerOne || game.GameState == GameState.P2_CHOOSING && !isPlayerOne)
                    game.GameState = GameState.BOTH_PICKING_PLAYER;
                else if (game.GameState == GameState.BOTH_CHOSING_CARDS && isPlayerOne)
                    game.GameState = GameState.P2_CHOOSING;
                else if (game.GameState == GameState.BOTH_CHOSING_CARDS && !isPlayerOne)
                    game.GameState = GameState.P1_CHOOSING;

                await _gameRepository.UpdateGameState(game, game.GameState);
                await _boardcardRepository.CreateBoardCards(newBoardCards);
                await transaction.CommitAsync();

                return game.GameState;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"Error while creating BoardCards for game with id {gameId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<int> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                BoardEntity board = await _boardRepository.GetBoardById(boardId);
                PlayerHasBoardPermission(playerId, board);

                IDictionary<int, bool> updateMap = boardCardUpdates.ToDictionary(update => update.BoardCardID, update => update.Active);
                IList<BoardCardEntity> boardCards = await _boardcardRepository.GetBoardCardsFromBoard(boardId);
                int boardcardsLeft = boardCards.Count(bc => bc.Active);

                await _boardcardRepository.UpdateBoardCardsActivity(updateMap, boardCards);
                await _boardRepository.UpdateBoardCardsLeft(board, boardcardsLeft);

                await transaction.CommitAsync();
                return boardcardsLeft;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<IEnumerable<BoardCardEntity>> GetBoardCardsFromBoard(int playerId, int boardId)
    {
        try
        {
            BoardEntity board = await _boardRepository.GetBoardById(boardId);
            PlayerHasBoardPermission(playerId, board);

            return await _boardcardRepository.GetBoardCardsFromBoard(boardId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
            throw;
        }
    }

    private void ValidatePlayerPermissions(int playerId, GameEntity game, IEnumerable<int> cardIds)
    {
        if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS && game.PlayerTwoID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS && cardIds.Count() != 20)
            throw new ArgumentException("Too few cardIds provided, needs 20");

        if ((game.GameState == GameState.BOTH_CHOSING_CARDS || game.GameState == GameState.P2_CHOOSING || game.GameState == GameState.P1_CHOOSING) && cardIds.Count() != 10)
            throw new ArgumentException("Too few cardIds provided, needs 10");

        if (game.GameState == GameState.P1_CHOOSING && game.PlayerTwoID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.GameState == GameState.P2_CHOOSING && game.PlayerOneID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");
    }

    public void PlayerHasBoardPermission(int playerId, BoardEntity board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }

    public void PlayerHasGamePermission(int playerId, GameEntity game)
    {
        if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}