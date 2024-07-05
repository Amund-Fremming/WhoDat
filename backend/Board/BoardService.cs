namespace BoardEntity;

public class BoardService(ILogger<IBoardService> logger, AppDbContext context, IBoardRepository boardRepository, IBoardCardRepository boardCardRepository, IGameRepository gameRepository, IPlayerRepository playerRepository) : IBoardService
{
    public readonly ILogger<IBoardService> _logger = logger;
    public readonly AppDbContext _context = context;
    public readonly IBoardRepository _boardRepository = boardRepository;
    public readonly IBoardCardRepository _boardCardRepository = boardCardRepository;
    public readonly IGameRepository _gameRepository = gameRepository;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateBoard(int playerId, int gameId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _playerRepository.GetPlayerById(playerId);
                await _gameRepository.GetGameById(gameId);

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
                PlayerHasBoardPermission(playerId, board);

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

    public async Task<State> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Board board = await _boardRepository.GetBoardById(boardId);
                Game game = await _gameRepository.GetGameById(gameId);

                PlayerHasBoardPermission(playerId, board);
                PlayerHasGamePermission(playerId, game);
                PlayerCanChooseCard(playerId, game, board);

                BoardCard boardCard = await _boardCardRepository.GetBoardCardById(boardCardId);
                await _boardRepository.ChooseBoardCard(board, boardCard);

                bool isPlayerOne = game.PlayerOneID == playerId;
                if (isPlayerOne && game.State == State.BOTH_PICKING_PLAYER)
                    game.State = State.P2_PICKING_PLAYER;

                if (!isPlayerOne && game.State == State.BOTH_PICKING_PLAYER)
                    game.State = State.P1_PICKING_PLAYER;

                if ((isPlayerOne && game.State == State.P1_PICKING_PLAYER) || (!isPlayerOne && game.State == State.P2_PICKING_PLAYER))
                    game.State = State.BOTH_PICKED_PLAYERS;

                await _gameRepository.UpdateGameState(game, game.State);
                await transaction.CommitAsync();
                return game.State;
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

    public async Task UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers)
    {
        try
        {
            Board board = await _boardRepository.GetBoardById(boardId);
            PlayerHasBoardPermission(playerId, board);

            await _boardRepository.UpdateBoardCardsLeft(board, activePlayers);
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
            Game game = await _gameRepository.GetGameById(gameId);
            PlayerHasGamePermission(playerId, game);

            Board playerOneBoard = game.Boards!.ElementAt(0);
            Board playerTwoBoard = game.Boards!.ElementAt(1);

            if (playerOneBoard.PlayerID == playerId)
                return playerOneBoard;

            if (playerTwoBoard.PlayerID == playerId && playerTwoBoard == null)
                return await CreatePlayerTwoBoard(playerId, game);

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
                PlayerCanGuessBoardCard(playerId, game);
                Board otherPlayersBoard = null!;

                if (game.Boards!.Count() < 2)
                    throw new NullReferenceException($"Players board is null in game {game.GameID}.");

                if (game.Boards!.ElementAt(0).PlayerID == playerId)
                    otherPlayersBoard = game.Boards!.ElementAt(1);

                if (game.Boards!.ElementAt(1).PlayerID == playerId)
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
        if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }

    public void PlayerHasBoardPermission(int playerId, Board board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }

    public void PlayerCanChooseCard(int playerId, Game game, Board board)
    {
        bool isPlayerOne = game.PlayerOneID == playerId;

        if (game.State != State.P1_PICKING_PLAYER && game.State != State.P2_PICKING_PLAYER && game.State != State.BOTH_PICKING_PLAYER)
            throw new InvalidOperationException("This action cannot be performed in this State");

        if ((isPlayerOne && game.State == State.P2_PICKING_PLAYER) || (!isPlayerOne && game.State == State.P1_PICKING_PLAYER))
            throw new InvalidOperationException("This action cannot be performed in this State");
    }

    public void PlayerCanGuessBoardCard(int playerId, Game game)
    {
        // TODO
        // throw new UnauthorizedAccessException($"Its not player {playerId}`s turn!");
    }

    private async Task<Board> CreatePlayerTwoBoard(int playerId, Game game)
    {
        Player player = await _playerRepository.GetPlayerById(playerId);
        Board playerOneBoard = game.Boards!.ElementAt(1);

        Board playerTwoBoard = new Board(playerId, game.GameID);
        List<BoardCard> tempBoardCards = new List<BoardCard>();

        foreach (BoardCard boardCard in playerOneBoard.BoardCards!)
        {
            BoardCard newBoardCard = new BoardCard(boardCard.BoardID, boardCard.CardID);
            tempBoardCards.Add(newBoardCard);
        }

        playerTwoBoard.BoardCards = tempBoardCards;

        await _boardRepository.CreateBoard(playerTwoBoard);
        return playerTwoBoard;
    }
}
