import * as signalR from '@microsoft/signalr';

import { HUB_ENDPOINT } from '@/src/Shared/objects/URL_PATHS';
import Result from '../Shared/objects/Result';
import { GameState } from './types/GameTypes';
import { IBoardCardUpdate } from './types/BoardTypes';

export const createConnection = (token: string): signalR.HubConnection => {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${HUB_ENDPOINT}`, {
      accessTokenFactory: () => token,
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();
};

export const startConnection = async (
  connection: signalR.HubConnection
): Promise<Result<boolean>> => {
  try {
    await connection.start();
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const stopConnection = async (
  connection: signalR.HubConnection
): Promise<void> => {
  try {
    await connection.stop();
  } catch (error) {
    console.error('Error while stopping connection: ', error);
  }
};

export const leaveGame = async (
  connection: signalR.HubConnection,
  gameId: number,
  doBroadcast: boolean
): Promise<void> => {
  try {
    await connection.invoke('LeaveGame', gameId, doBroadcast);
  } catch (error) {
    console.error(error);
  }
};

export const joinGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('JoinGame', gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const subscribeToGameAsHost = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('SubscribeToGameAsHost', gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const updateGameState = async (
  connection: signalR.HubConnection,
  gameId: number,
  gameState: GameState
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('UpdateGameState', gameId, gameState);
    return Result.ok(true);
  } catch (error) {
    console.error('errrrroooor' + error);
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const sendMessage = async (
  connection: signalR.HubConnection,
  gameId: number,
  messageText: string
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('SendMessage', gameId, messageText);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const guessBoardCard = async (
  connection: signalR.HubConnection,
  gameId: number,
  boardCardId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('GuessBoardCard', gameId, boardCardId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const updateBoardCardsActivity = async (
  connection: signalR.HubConnection,
  gameId: number,
  boardId: number,
  boardCardUpdates: Array<IBoardCardUpdate>
): Promise<Result<boolean>> => {
  try {
    await connection.invoke(
      'UpdateBoardCardsActivity',
      gameId,
      boardId,
      boardCardUpdates
    );
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const createBoardCards = async (
  connection: signalR.HubConnection,
  gameId: number,
  cardIds: Array<number>
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('CreateBoardCards', gameId, cardIds);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const chooseBoardCard = async (
  connection: signalR.HubConnection,
  gameId: number,
  boardId: number,
  boardCardId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('ChooseBoardCard', gameId, boardId, boardCardId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};

export const startGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke('StartGame', gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure('Falied to connect, check your wifi');
  }
};
