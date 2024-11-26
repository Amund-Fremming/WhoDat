using Backend.Features.Game;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Board
{
    public static class BoardValidation
    {
        public static Result HasGamePermission(int playerId, GameEntity game)
        {
            if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission."), "You dont have persmission to this action.");

            return Result.Ok();
        }

        public static Result HasBoardPermission(int playerId, BoardEntity board)
        {
            if (board.PlayerID != playerId)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission."), "You dont have persmission to this action.");

            return Result.Ok();
        }

        public static Result CanChooseCard(int playerId, GameEntity game, BoardEntity board)
        {
            bool isPlayerOne = game.PlayerOneID == playerId;

            if (game.GameState != GameState.P1_PICKING_PLAYER && game.GameState != GameState.P2_PICKING_PLAYER && game.GameState != GameState.BOTH_PICKING_PLAYER)
                return new Error(new InvalidOperationException("This action cannot be performed in this State"), "Cannot perform this action in current game state.");

            if (isPlayerOne && game.GameState == GameState.P2_PICKING_PLAYER || !isPlayerOne && game.GameState == GameState.P1_PICKING_PLAYER)
                return new Error(new InvalidOperationException("This action cannot be performed in this State"), "Cannot perform this action in current game state.");

            return Result.Ok();
        }

        public static Result CanGuessBoardCard(int playerId, GameEntity game)
        {
            bool isPlayersTurn = game.GameState == GameState.P1_TURN_STARTED && playerId == game.PlayerOneID || game.GameState == GameState.P2_TURN_STARTED && playerId == game.PlayerTwoID;

            if (!isPlayersTurn)
                return new Error(new UnauthorizedAccessException($"Its not player {playerId}`s turn!"), "Not your turn.");

            return Result.Ok();
        }
    }
}