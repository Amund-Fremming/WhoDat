namespace BoardCardEntity;

public class BoardCardService(AppDbContext context, ILogger<IBoardCardService> logger,
        IBoardCardRepository boardcardRepository, IBoardRepository boardRepository, ICardRepository cardRepository, IGameRepository gameRepository) : IBoardCardService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IBoardCardService> _logger = logger;
    public readonly IBoardCardRepository _boardcardRepository = boardcardRepository;
    public readonly IBoardRepository _boardRepository = boardRepository;
    public readonly ICardRepository _cardRepository = cardRepository;
    public readonly IGameRepository _gameRepository = gameRepository;

    public async Task<State> CreateBoardCards(int playerId, int gameId, IEnumerable<int> cardIds)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Game game = await _gameRepository.GetGameById(gameId);
                if (game.State != State.ONLY_HOST_CHOSING_CARDS || game.State != State.BOTH_CHOSING_CARDS || game.State != State.P1_CHOOSING || game.State != State.P2_CHOOSING)
                    throw new ArgumentException("This action is not allowed");

                PlayerHasGamePermission(playerId, game);
                ValidatePlayerPermissions(playerId, game, cardIds);

                int boardId = game.Boards!.ElementAt(0).BoardID;

                if (game.Boards!.ElementAt(0) == null)
                {
                    Board board = new Board(playerId, gameId);
                    boardId = await _boardRepository.CreateBoard(board);
                }

                if (game.State == State.ONLY_HOST_CHOSING_CARDS)
                    cardIds = cardIds.Take(20);
                else
                    cardIds = cardIds.Take(10);

                IEnumerable<BoardCard> newBoardCards = cardIds.Select(cardId => new BoardCard(boardId, cardId)).ToList();

                bool isPlayerOne = game.PlayerOneID == playerId;
                if (game.State == State.P1_CHOOSING && isPlayerOne || game.State == State.P2_CHOOSING && !isPlayerOne)
                    throw new ArgumentException("Player cannot create more BoardCards!");
                else if (game.State == State.ONLY_HOST_CHOSING_CARDS || game.State == State.P1_CHOOSING && isPlayerOne || game.State == State.P2_CHOOSING && !isPlayerOne)
                    game.State = State.BOTH_PICKING_PLAYER;
                else if (game.State == State.BOTH_CHOSING_CARDS && isPlayerOne)
                    game.State = State.P2_CHOOSING;
                else if (game.State == State.BOTH_CHOSING_CARDS && !isPlayerOne)
                    game.State = State.P1_CHOOSING;

                await _gameRepository.UpdateGameState(game, game.State);
                await _boardcardRepository.CreateBoardCards(newBoardCards);
                await transaction.CommitAsync();

                return game.State;
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
                Board board = await _boardRepository.GetBoardById(boardId);
                PlayerHasBoardPermission(playerId, board);

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
            PlayerHasBoardPermission(playerId, board);

            return await _boardcardRepository.GetBoardCardsFromBoard(boardId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error while updating BoardCard for Board with id {boardId}. (BoardCardService)");
            throw;
        }
    }

    private void ValidatePlayerPermissions(int playerId, Game game, IEnumerable<int> cardIds)
    {
        if (game.State == State.ONLY_HOST_CHOSING_CARDS && game.PlayerOneID != playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.State == State.ONLY_HOST_CHOSING_CARDS && cardIds.Count() != 20)
            throw new ArgumentException("Too few cardIds provided, needs 20");

        if ((game.State == State.BOTH_CHOSING_CARDS || game.State == State.P2_CHOOSING || game.State == State.P1_CHOOSING) && cardIds.Count() != 10)
            throw new ArgumentException("Too few cardIds provided, needs 10");

        if (game.State == State.P1_CHOOSING && game.PlayerTwoID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");

        if (game.State == State.P2_CHOOSING && game.PlayerOneID == playerId)
            throw new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards");
    }


    public void PlayerHasBoardPermission(int playerId, Board board)
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
