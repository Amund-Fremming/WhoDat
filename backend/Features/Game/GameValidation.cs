using Backend.Features.Board;
using Backend.Features.Shared.Enums;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Game
{
    public static class GameValidation
    {
        public static Result ValidState(GameEntity game)
        {
            if (game.GameState != GameState.BOTH_PICKED_PLAYERS)
                return new Error(new ArgumentOutOfRangeException("Game cannot start at this state!"), "Cannot do this action in this game state.");

            if (game.PlayerOneID == null || game.PlayerTwoID == null)
                return new Error(new ArgumentOutOfRangeException("Game is missing one player"), "You need two players in a game.");

            if (game.Boards == null || game.Boards.Count() < 2)
                return new Error(new ArgumentNullException("Missing one or two boards"), "One or both of the boards have not been created.");

            BoardEntity playerOneBoard = game.Boards.ElementAt(0);
            BoardEntity playerTwoBoard = game.Boards.ElementAt(1);
            if (playerTwoBoard == null)
                return new Error(new ArgumentOutOfRangeException("One player two has not a board created."), "Player two is missing a board.");

            if (playerOneBoard.ChosenCardID == null || playerTwoBoard.ChosenCardID == null)
                return new Error(new ArgumentOutOfRangeException("Both players have not choosen their playing card."), "Both players have not choosen their playing card.");

            return Result.Ok();
        }

        public static Result HasPermission(int playerId, GameEntity game)
        {
            if (playerId != game.PlayerOneID && playerId != game.PlayerTwoID)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission (GameService)"), "You dont have permission for this action.");

            return Result.Ok();
        }

        public static Result CanUpdateGame(int playerId, GameEntity game)
        {
            bool isPlayerOne = game.PlayerOneID == playerId;
            if (isPlayerOne && game.GameState != GameState.P1_ASK_REPLIED && game.GameState != GameState.P1_TURN_STARTED && game.GameState != GameState.P1_GUESS_REPLIED ||
                !isPlayerOne && game.GameState != GameState.P2_ASK_REPLIED && game.GameState != GameState.P2_TURN_STARTED && game.GameState != GameState.P2_GUESS_REPLIED)
                return new Error(new UnauthorizedAccessException($"Player with id {playerId} does not have permission to update the state (GameService)"), "You dont have permission for this action.");

            return Result.Ok();
        }
    }
}