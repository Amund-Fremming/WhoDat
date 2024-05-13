using BoardEntity;
using CardEntity;
using Data;
using GameEntity;
using Enum;

namespace BoardCardEntity;

public class BoardCardService(AppDbContext context, ILogger<BoardCardService> logger,
        BoardCardRepository boardcardRepository, BoardRepository boardRepository, CardRepository cardRepository, GameRepository gameRepository) : IBoardCardService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<BoardCardService> _logger = logger;
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;
    public readonly BoardRepository _boardRepository = boardRepository;
    public readonly CardRepository _cardRepository = cardRepository;
    public readonly GameRepository _gameRepository = gameRepository;

    public async Task<State> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                int boardId = game.Boards!.ElementAt(0).BoardID;

                PlayerHasGamePermission(playerId, game);
                ValidatePlayerPermissions(playerId, game, cardIds);

                if (game.State == State.ONLY_HOST_CHOSING_CARDS)
                    game.State = State.BOTH_PICKING_PLAYER;

                if (game.State == State.BOTH_CHOSING_CARDS)
                    game.State = game.PlayerOneID == playerId ? State.P2_CHOOSING : State.P1_CHOOSING;

                if (game.State == State.P1_CHOOSING || game.State == State.P2_CHOOSING)
                    game.State = State.BOTH_PICKING_PLAYER;


                IEnumerable<BoardCard> newBoardCards = cardIds.Select(cardId => new BoardCard(boardId, cardId)).ToList();

                await _gameRepository.UpdateGameState(game, game.State);
                await _boardcardRepository.CreateBoardCards(newBoardCards);
                await transaction.CommitAsync();

                return game.State;
            }
            catch (Exception e)
            {
                // ADD HANDLING
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
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasPermission(playerId, board);

                IDictionary<int, bool> updateMap = boardCardUpdates.ToDictionary(update => update.BoardCardID, update => update.Active);
                IList<BoardCard> boardCards = await _boardcardRepository.GetBoardCardsFromBoard(boardId);
                int boardcardsLeft = boardCards.Count(bc => bc.Active);

                await _boardcardRepository.UpdateBoardCardsActivity(updateMap, boardCards);
                await _boardRepository.UpdateBoardCardsLeft(board, boardcardsLeft);

                await transaction.CommitAsync();
                return boardcardsLeft;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<IEnumerable<BoardCard>> GetBoardCardsFromBoard(int playerId, int boardId)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasPermission(playerId, board);

            return await _boardcardRepository.GetBoardCardsFromBoard(boardId);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
            throw;
        }
    }

    private void ValidatePlayerPermissions(int playerId, Game game, IEnumerable<int> cardIds)
    {
        if (game.State == State.ONLY_HOST_CHOSING_CARDS && game.PlayerOneID != playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.State == State.ONLY_HOST_CHOSING_CARDS && cardIds.Count() != 40)
            throw new ArgumentException("Too few cardIds provided, needs 40");

        if ((game.State == State.BOTH_CHOSING_CARDS || game.State == State.P2_CHOOSING || game.State == State.P1_CHOOSING) && cardIds.Count() != 20)
            throw new ArgumentException("Too few cardIds provided, needs 20");

        if (game.State == State.P1_CHOOSING && game.PlayerTwoID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.State == State.P2_CHOOSING && game.PlayerOneID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");
    }


    public void PlayerHasPermission(int playerId, Board board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }

    public void PlayerHasGamePermission(int playerId, Game game)
    {
        if (game.PlayerOneID != playerId || game.PlayerTwoID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}

