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

    public async Task<Result> DeleteBoard(int playerId, int boardId)
    {
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (result.IsError)
                return result;

            var board = result.Data;
            BoardValidation.HasBoardPermission(playerId, board);

            return await _boardRepository.DeleteBoard(board);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(DeleteBoard)");
            return new Error(e, "Failed to delete board.");
        }
    }

    public async Task<Result<GameState>> ChooseBoardCard(int playerId, int gameId, int boardId, int boardCardId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var boardResult = await _boardRepository.GetBoardById(boardId);
            if (boardResult.IsError)
                return boardResult.Error;

            var gameResult = await _gameRepository.GetGameById(gameId);
            if (boardResult.IsError)
                return gameResult.Error;

            var board = boardResult.Data;
            var game = gameResult.Data;

            var result = BoardValidation.HasBoardPermission(playerId, board)
                & BoardValidation.HasGamePermission(playerId, game)
                & BoardValidation.CanChooseCard(playerId, game, board);

            if (result.IsError)
                return result.Error;

            var bcResult = await _boardCardRepository.GetBoardCardById(boardCardId);
            if (bcResult.IsError)
                return bcResult.Error;

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
            return new Error(e, "Failed to choose boardcard.");
        }
    }

    public async Task<Result> UpdateBoardCardsLeft(int playerId, int boardId, int activePlayers)
    {
        try
        {
            var result = await _boardRepository.GetBoardById(boardId);
            if (result.IsError)
                return result;

            var board = result.Data;
            var validation = BoardValidation.HasBoardPermission(playerId, board);
            if (validation.IsError)
                return result;

            return await _boardRepository.UpdateBoardCardsLeft(board, activePlayers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsLeft)");
            return new Error(e, "Failed to update board cards left.");
        }
    }

    public async Task<Result<BoardEntity>> GetBoardWithBoardCards(int playerId, int gameId)
    {
        try
        {
            var result = await _gameRepository.GetGameById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var validation = BoardValidation.HasGamePermission(playerId, game);

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
            _logger.LogError(e, "(GetBoardWithBoardCards) - could be player 2 board not created.");
            return new Error(e, "Failed to get board.");
        }
    }

    public async Task<Result<GameState>> GuessBoardCard(int playerId, int gameId, int boardCardId)
    {
        try
        {
            var result = await _gameRepository.GetGameById(gameId);
            if (result.IsError)
                return result.Error;

            var game = result.Data;
            var validation = BoardValidation.CanGuessBoardCard(playerId, game);
            if (validation.IsError)
                return validation.Error;

            BoardEntity otherPlayersBoard = null!;

            if (game.Boards!.Count() < 2)
                throw new NullReferenceException($"Players board is null in game {game.GameID}.");

            if (game.Boards!.ElementAt(0).PlayerID == playerId)
                otherPlayersBoard = game.Boards!.ElementAt(1);

            if (game.Boards!.ElementAt(1).PlayerID == playerId)
                otherPlayersBoard = game.Boards!.ElementAt(0);

            var guessResult = await _boardCardRepository.GetBoardCardById(boardCardId);
            if (guessResult.IsError)
                return guessResult.Error;

            var guessedCard = guessResult.Data;
            if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                game.GameState = GameState.P1_WON;
            if (guessedCard.BoardCardID == otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                game.GameState = GameState.P2_WON;
            if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerTwoID)
                game.GameState = GameState.P1_TURN_STARTED;
            if (guessedCard.BoardCardID != otherPlayersBoard.ChosenCard!.BoardCardID && playerId == game.PlayerOneID)
                game.GameState = GameState.P2_TURN_STARTED;

            await _gameRepository.UpdateGame(game);
            return game.GameState;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GuessBoardCard)");
            return new Error(e, "Failed to guess board card.");
        }
    }

    private async Task<Result<BoardEntity>> CreatePlayerTwoBoard(int playerId, GameEntity game)
    {
        var result = await _playerRepository.GetPlayerById(playerId);
        if (result.IsError)
            return result.Error;

        BoardEntity playerOneBoard = game.Boards!.ElementAt(0);
        BoardEntity playerTwoBoard = new(playerId, game.GameID);
        List<BoardCardEntity> tempBoardCards = [];

        foreach (BoardCardEntity boardCard in playerOneBoard.BoardCards!)
        {
            BoardCardEntity newBoardCard = new(boardCard.BoardID, boardCard.CardID);
            tempBoardCards.Add(newBoardCard);
        }

        playerTwoBoard.BoardCards = tempBoardCards;

        var createResult = await _boardRepository.CreateBoard(playerTwoBoard);
        if (createResult.IsError)
            return result.Error;

        return playerTwoBoard;
    }
}