using Backend.Features.BoardCard;
using Backend.Features.Database;
using Backend.Features.Game;
using Backend.Features.Player;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board;

public class BoardService(ILogger<IBoardService> logger, AppDbContext context, IBoardRepository boardRepository, IBoardCardRepository boardCardRepository, IGameRepository gameRepository, IPlayerRepository playerRepository) : IBoardService
{
    public readonly ILogger<IBoardService> _logger = logger;
    public readonly AppDbContext _context = context;
    public readonly IBoardRepository _boardRepository = boardRepository;
    public readonly IBoardCardRepository _boardCardRepository = boardCardRepository;
    public readonly IGameRepository _gameRepository = gameRepository;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    //public async Task<int> CreateBoard(int playerId, int gameId)
    //{
    //    try
    //    {
    //        await _playerRepository.GetPlayerById(playerId);
    //        await _gameRepository.GetGameById(gameId);

    //        BoardEntity board = new(playerId, gameId);
    //        int boardId = await _boardRepository.CreateBoard(board);

    //        return boardId;
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e.Message, $"Error while creating Board. (BoardService)");
    //        throw;
    //    }
    //}

    public async Task<Result> DeleteBoard(int playerId, int boardId)
    {
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (!result.IsSuccess)
                return result.RemoveType();

            var board = result.Data;
            PlayerHasBoardPermission(playerId, board);

            return await _boardRepository.DeleteBoard(board);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteBoard)");
            return (e, "Failed to delete board. Please try again later.");
        }
    }

    public async Task<Result<GameState>> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var boardResult = await _boardRepository.GetBoardById(boardId);
            if (!boardResult.IsSuccess)
                return (boardResult.Exception, boardResult.Message);

            var gameResult = await _gameRepository.GetGameById(gameId);
            if (!boardResult.IsSuccess)
                return (gameResult.Exception, gameResult.Message);

            var board = boardResult.Data;
            var game = gameResult.Data;

            PlayerHasBoardPermission(playerId, board);
            PlayerHasGamePermission(playerId, game);
            PlayerCanChooseCard(playerId, game, board);

            var bcResult = await _boardCardRepository.GetBoardCardById(boardCardId);
            if (!bcResult.IsSuccess)
                return (bcResult.Exception, bcResult.Message);

            var boardCard = bcResult.Data;
            await _boardRepository.ChooseBoardCard(board, boardCard);

            bool isPlayerOne = game.PlayerOneID == playerId;
            if (isPlayerOne && game.GameState == GameState.BOTH_PICKING_PLAYER)
                game.GameState = GameState.P2_PICKING_PLAYER;

            if (!isPlayerOne && game.GameState == GameState.BOTH_PICKING_PLAYER)
                game.GameState = GameState.P1_PICKING_PLAYER;

            if (isPlayerOne && game.GameState == GameState.P1_PICKING_PLAYER || !isPlayerOne && game.GameState == GameState.P2_PICKING_PLAYER)
                game.GameState = GameState.BOTH_PICKED_PLAYERS;

            await _gameRepository.UpdateGameState(game, game.GameState);
            await transaction.CommitAsync();
            return game.GameState;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(ChooseBoardCard)");
            await transaction.RollbackAsync();
            return (e, "Failed to choose boardcard. Please try again later.");
        }
    }

    public async Task<Result> UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers)
    {
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (!result.IsSuccess)
                return result.RemoveType();

            var board = result.Data;
            PlayerHasBoardPermission(playerId, board);

            return await _boardRepository.UpdateBoardCardsLeft(board, activePlayers);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error updating card on Board with id {boardId}. (BoardService)");
            throw;
        }
    }

    public async Task<BoardEntity> GetBoardWithBoardCards(int playerId, int gameId)
    {
        try
        {
            GameEntity game = await _gameRepository.GetGameById(gameId);
            PlayerHasGamePermission(playerId, game);

            BoardEntity playerOneBoard = game.Boards!.ElementAt(0);

            if (playerOneBoard.PlayerID == playerId)
                return playerOneBoard;

            if (game.Boards!.Count() <= 1)
                return await CreatePlayerTwoBoard(playerId, game);

            BoardEntity playerTwoBoard = game.Boards!.ElementAt(1);

            if (playerTwoBoard.PlayerID == playerId)
                return playerTwoBoard;

            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error fetching Board from game with id {gameId}. (BoardService)");
            throw;
        }
    }

    public async Task<GameState> GuessBoardCard(int playerId, int gameId, int boardCardId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                GameEntity game = await _gameRepository.GetGameById(gameId);
                PlayerCanGuessBoardCard(playerId, game);
                BoardEntity otherPlayersBoard = null!;

                if (game.Boards!.Count() < 2)
                    throw new NullReferenceException($"Players board is null in game {game.GameID}.");

                if (game.Boards!.ElementAt(0).PlayerID == playerId)
                    otherPlayersBoard = game.Boards!.ElementAt(1);

                if (game.Boards!.ElementAt(1).PlayerID == playerId)
                    otherPlayersBoard = game.Boards!.ElementAt(0);

                BoardCardEntity guessedCard = await _boardCardRepository.GetBoardCardById(boardCardId);
                PlayerHasGamePermission(playerId, game);

                if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                    game.GameState = GameState.P1_WON;
                if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                    game.GameState = GameState.P2_WON;
                if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                    game.GameState = GameState.P1_TURN_STARTED;
                if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                    game.GameState = GameState.P2_TURN_STARTED;

                await _gameRepository.UpdateGame(game);
                await transaction.CommitAsync();
                return game.GameState;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"Error taking in guess on game wiht id {gameId}. (BoardService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public void PlayerHasGamePermission(int playerId, GameEntity game)
    {
        if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }

    public void PlayerHasBoardPermission(int playerId, BoardEntity board)
    {
        if (board.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission (BoardService)");
        }
    }

    public void PlayerCanChooseCard(int playerId, GameEntity game, BoardEntity board)
    {
        bool isPlayerOne = game.PlayerOneID == playerId;

        if (game.GameState != GameState.P1_PICKING_PLAYER && game.GameState != GameState.P2_PICKING_PLAYER && game.GameState != GameState.BOTH_PICKING_PLAYER)
            throw new InvalidOperationException("This action cannot be performed in this State");

        if (isPlayerOne && game.GameState == GameState.P2_PICKING_PLAYER || !isPlayerOne && game.GameState == GameState.P1_PICKING_PLAYER)
            throw new InvalidOperationException("This action cannot be performed in this State");
    }

    public void PlayerCanGuessBoardCard(int playerId, GameEntity game)
    {
        bool isPlayersTurn = game.GameState == GameState.P1_TURN_STARTED && playerId == game.PlayerOneID || game.GameState == GameState.P2_TURN_STARTED && playerId == game.PlayerTwoID;

        if (!isPlayersTurn)
            throw new UnauthorizedAccessException($"Its not player {playerId}`s turn!");
    }

    private async Task<BoardEntity> CreatePlayerTwoBoard(int playerId, GameEntity game)
    {
        PlayerEntity player = await _playerRepository.GetPlayerById(playerId);

        BoardEntity playerOneBoard = playerOneBoard = game.Boards!.ElementAt(0);

        BoardEntity playerTwoBoard = new(playerId, game.GameID);
        List<BoardCardEntity> tempBoardCards = [];

        foreach (BoardCardEntity boardCard in playerOneBoard.BoardCards!)
        {
            BoardCardEntity newBoardCard = new(boardCard.BoardID, boardCard.CardID);
            tempBoardCards.Add(newBoardCard);
        }

        playerTwoBoard.BoardCards = tempBoardCards;

        await _boardRepository.CreateBoard(playerTwoBoard);
        return playerTwoBoard;
    }
}