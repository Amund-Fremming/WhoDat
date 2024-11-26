import { GAME_ENDPOINT } from "../Shared/domain/URL_PATHS";
import { IGame } from "@/src/Game/types/GameTypes";
import { Result } from "@/src/Shared/domain/Result";

export const createGame = async (
  game: IGame,
  token: string
): Promise<Result<number>> => {
  try {
    const response = await fetch(`${GAME_ENDPOINT}/games`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(game),
    });

    if (!response.ok) {
      console.error("createGame: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const gameId = await response.text();
    return Result.ok(parseInt(gameId));
  } catch (error) {
    console.error("(createGame)" + error);
    return Result.failure("Something went wrong.");
  }
};

export const getBoardWithBoardCards = async (
  gameId: number,
  token: string
): Promise<Result<IBoard>> => {
  try {
    const response = await fetch(`${GAME_ENDPOINT}/games/${gameId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      console.error("getBoardWithBoardCards: response was not 200.");
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const boardData = await response.json();
    return Result.ok(boardData);
  } catch (error) {
    console.error("(getBoardWithBoardCards)" + error);
    return Result.failure("Something went wrong.");
  }
};
