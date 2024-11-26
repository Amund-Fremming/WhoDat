import { GAME_ENDPOINT } from "../../shared/domain/URL_PATHS";
import { IGame } from "@/src/Game/types/GameTypes";

export const createGame = async (game: IGame, token: string) => {
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
      throw new Error(`Error in creating game, Status: ${response.status}`);
    }

    const gameId = await response.text();
    console.log(`Game ${gameId} Created!`); // Log the gameId or return it
    return parseInt(gameId);
  } catch (error) {
    console.error("Error when creating a game: " + error);
    throw error;
  }
};

export const getBoardWithBoardCards = async (gameId: number, token: string) => {
  try {
    const response = await fetch(`${GAME_ENDPOINT}/games/${gameId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(
        `Error in getting game board and cards, Status: ${response.status}`
      );
    }

    const boardData = await response.json();
    console.log(boardData); // Log or return the board data
    return boardData;
  } catch (error) {
    console.error("Error when fetching game board and cards: " + error);
    throw error;
  }
};
