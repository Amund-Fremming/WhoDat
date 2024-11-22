using Backend.Features.Board;
using Backend.Features.Card;
using Backend.Features.Database;
using Backend.Features.Game;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

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

    public async Task<Result<GameState>> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds)
    {
        try
        {
            var result = await _gameRepository.GetGameById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;

            var validation = BoardCardValidation.HasGamePermission(playerId, game)
                & BoardCardValidation.PlayerPermissions(playerId, game, cardIds);

            if (validation.IsError)
                return validation.Error;

            int boardId = game.Boards!.ElementAt(0).BoardID;
            if (game.Boards!.ElementAt(0) == null)
            {
                BoardEntity board = new(playerId, gameId);
                var boardResult = await _boardRepository.CreateBoard(board);
                if (boardResult.IsError)
                    return boardResult.Error;
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

            return game.GameState;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateBoardCards)");
            return new Error(e, "Failed to create boardcards.");
        }
    }

    public async Task<Result> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (result.IsError)
                return result.Error;

            var board = result.Data;
            var validation = BoardCardValidation.HasBoardPermission(playerId, board);
            if (validation.IsError)
                return validation.Error;

            IDictionary<int, bool> updateMap = boardCardUpdates.ToDictionary(update => update.BoardCardID, update => update.Active);
            var bcResult = await _boardcardRepository.GetBoardCardsFromBoard(boardId);
            if (bcResult.IsError)
                return bcResult.Error;

            var boardCards = bcResult.Data;
            int boardcardsLeft = boardCards.Count(bc => bc.Active);

            await _boardcardRepository.UpdateBoardCardsActivity(updateMap, boardCards);
            await _boardRepository.UpdateBoardCardsLeft(board, boardcardsLeft);

            await transaction.CommitAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int playerId, int boardId)
    {
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (result.IsError)
                return result.Error;

            var board = result.Data;
            var validation = BoardCardValidation.HasBoardPermission(playerId, board);
            if (validation.IsError)
                return validation.Error;

            return Result<IEnumerable<BoardCardEntity>>.Ok(board.BoardCards);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBardCardsFromBoard) - board could be null, boardcards might not be attached.");
            return new Error(e, "Failed to get board cards for your board.");
        }
    }
}