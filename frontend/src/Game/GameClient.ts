import { GAME_ENDPOINT } from '../Shared/assets/constants/URL_PATHS';
import { GameState, IGame } from '@/src/Game/types/GameTypes';
import Result from '../Shared/objects/Result';
import { IBoard } from './types/BoardTypes';

export const createGame = async (
  gameState: GameState,
  token: string
): Promise<Result<number>> => {
  try {
    const response = await fetch(`${GAME_ENDPOINT}/games/${gameState}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      console.error('createGame: response was not 200.');
      console.log(response);
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const gameId = await response.json();
    return Result.ok(parseInt(gameId));
  } catch (error) {
    console.error('(createGame fe)' + error);
    return Result.failure('Something went wrong.');
  }
};

export const getBoardWithBoardCards = async (
  gameId: number,
  token: string
): Promise<Result<IBoard>> => {
  try {
    const response = await fetch(`${GAME_ENDPOINT}/games/${gameId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      console.error('getBoardWithBoardCards: response was not 200.');
      const errorMessage = await response.json();
      return Result.failure(errorMessage);
    }

    const boardData = await response.json();
    return Result.ok(boardData);
  } catch (error) {
    console.error('(getBoardWithBoardCards)' + error);
    return Result.failure('Something went wrong.');
  }
};
