using Backend.Features.Board;
using Backend.Features.Game;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.BoardCard
{
    public static class BoardCardValidation
    {
        public static Result PlayerPermissions(int playerId, GameEntity game, IEnumerable<int> cardIds)
        {
            if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS && game.PlayerTwoID == playerId)
                return new Error(new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"), "You dont have permission to this action.");

            if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS && cardIds.Count() != 20)
                return new Error(new ArgumentException("Too few cardIds provided, needs 20"), "Please provide 20 cards.");

            if ((game.GameState == GameState.BOTH_CHOSING_CARDS || game.GameState == GameState.P2_CHOOSING || game.GameState == GameState.P1_CHOOSING) && cardIds.Count() != 10)
                return new Error(new ArgumentException("Too few cardIds provided, needs 10"), "Please provide 10 cards.");

            if (game.GameState == GameState.P1_CHOOSING && game.PlayerTwoID == playerId)
                return new Error(new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"), "You dont have permission to this action.");

            if (game.GameState == GameState.P2_CHOOSING && game.PlayerOneID == playerId)
                return new Error(new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"), "You dont have permission to this action.");

            return Result.Ok();
        }

        public static Result HasBoardPermission(int playerId, BoardEntity board)
        {
            if (board.PlayerID != playerId)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission"), "You dont have persmission to this action.");

            return Result.Ok();
        }

        public static Result HasGamePermission(int playerId, GameEntity game)
        {
            if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission"), "You dont have permission to this action.");

            return Result.Ok();
        }
    }
}