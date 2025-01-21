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
            return game.GameState switch
            {
                GameState.ONLY_HOST_CHOSING_CARDS when game.PlayerTwoID == playerId => new Error(
                    new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"),
                    "You dont have permission to this action."),
                GameState.ONLY_HOST_CHOSING_CARDS when cardIds.Count() != 20 => new Error(
                    new ArgumentException("Too few cardIds provided, needs 20"), "Please provide 20 cards."),
                GameState.BOTH_CHOSING_CARDS or GameState.P2_CHOOSING or GameState.P1_CHOOSING when
                    cardIds.Count() != 10 => new Error(new ArgumentException("Too few cardIds provided, needs 10"),
                        "Please provide 10 cards."),
                GameState.P1_CHOOSING when game.PlayerTwoID == playerId => new Error(
                    new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"),
                    "You dont have permission to this action."),
                GameState.P2_CHOOSING when game.PlayerOneID == playerId => new Error(
                    new UnauthorizedAccessException($"Player {playerId} does not have permission to create cards"),
                    "You dont have permission to this action."),
                _ => Result.Ok()
            };
        }

        public static Result HasBoardPermission(int playerId, BoardEntity board) =>
            board.PlayerID != playerId ? new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission"), "You dont have persmission to this action.") : Result.Ok();

        public static Result HasGamePermission(int playerId, GameEntity game)
        {
            if (game.PlayerOneID != playerId && game.PlayerTwoID != playerId)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission"), "You dont have permission to this action.");

            return Result.Ok();
        }
    }
}