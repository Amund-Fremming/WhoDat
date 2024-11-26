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
            var result = await _gameRepository.GetById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;

            var validation = BoardCardValidation.HasGamePermission(playerId, game)
                & BoardCardValidation.PlayerPermissions(playerId, game, cardIds);

            if (validation.IsError)
                return validation.Error;

            int boardId = game.Boards!.ElementAt(0).ID;
            if (game.Boards!.ElementAt(0) == null)
            {
                BoardEntity board = new(playerId, gameId);
                var boardResult = await _boardRepository.Create(board);
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

    public async Task<Result<int>> UpdateBoardCardsActivity(int playerId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _boardRepository.GetById(boardId);
            if (result.IsError)
                return result.Error;

            var board = result.Data;
            var validation = BoardCardValidation.HasBoardPermission(playerId, board);
            if (validation.IsError)
                return validation.Error;

            var bcResult = await _boardcardRepository.GetBoardCardsFromBoard(boardId);
            if (bcResult.IsError)
                return bcResult.Error;

            var boardCards = bcResult.Data;
            int boardcardsLeft = boardCards.Count(bc => bc.Active);

            IDictionary<int, bool> updateMap = boardCardUpdates.ToDictionary(update => update.BoardCardID, update => update.Active);
            var combinedResult = await _boardcardRepository.UpdateBoardCardsActivity(updateMap, boardCards)
                & await _boardRepository.UpdateBoardCardsLeft(board, boardcardsLeft);

            if (combinedResult.IsError)
            {
                await transaction.RollbackAsync();
                return combinedResult.Error;
            }

            await transaction.CommitAsync();
            return boardcardsLeft;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsActivity)");
            await transaction.RollbackAsync();
            return new Error(e, "Failed updating board cards activity.");
        }
    }

    public async Task<Result<IEnumerable<BoardCardEntity>>> GetBoardCardsFromBoard(int playerId, int boardId)
    {
        try
        {
            var result = await _boardRepository.GetById(boardId);
            if (result.IsError)
                return result.Error;

            var board = result.Data;
            var validation = BoardCardValidation.HasBoardPermission(playerId, board);
            if (validation.IsError)
                return validation.Error;

            if (board.BoardCards == null)
                return new Error(new NullReferenceException("Boards boardcards collection is null."), "Board does not have any boardcards.");

            return Result<IEnumerable<BoardCardEntity>>.Ok(board.BoardCards);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GetBardCardsFromBoard) - board could be null, boardcards might not be attached.");
            return new Error(e, "Failed to get board cards for your board.");
        }
    }
}