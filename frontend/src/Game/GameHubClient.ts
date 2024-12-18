import * as signalR from "@microsoft/signalr";

import { HUB_ENDPOINT } from "@/src/Shared/domain/URL_PATHS";
import Result from "../Shared/domain/Result";
import { GameState } from "./types/GameTypes";
import { IBoardCardUpdate } from "./types/BoardTypes";

export const createConnection = (): signalR.HubConnection => {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${HUB_ENDPOINT}`)
    .configureLogging(signalR.LogLevel.Information)
    .build();
};

export const startConnection = async (
  connection: signalR.HubConnection
): Promise<Result<boolean>> => {
  try {
    await connection.start();
    console.log("Connection started");
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const stopConnection = async (
  connection: signalR.HubConnection
): Promise<void> => {
  try {
    await connection.stop();
    console.log("Connection stopped");
  } catch (error) {
    console.error("Error while stopping connection: ", error);
  }
};

export const leaveGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<void> => {
  try {
    await connection.invoke("LeaveGame", gameId);
    console.log("Left game:", gameId);
  } catch (error) {}
};

export const joinGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("JoinGame", gameId);
    console.log("Joined game:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const subscribeToGameAsHost = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("SubscribeToGameAsHost", gameId);
    console.log("Host subscribed to game:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const updateGameState = async (
  connection: signalR.HubConnection,
  gameState: GameState
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("UpdateGameState", gameState);
    console.log("UpdateGameState:", gameState);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const sendMessage = async (
  connection: signalR.HubConnection,
  messageText: string
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("SendMessage", messageText);
    console.log("SendMessage:", messageText);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const guessBoardCard = async (
  connection: signalR.HubConnection,
  boardCardId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("GuessBoardCard", boardCardId);
    console.log("GuessBoardCard:", boardCardId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const updateBoardCardsActivity = async (
  connection: signalR.HubConnection,
  gameId: number,
  boardId: number,
  boardCardUpdates: Array<IBoardCardUpdate>
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("UpdateBoardCardsActivity");
    console.log("UpdateBoardCardsActivity:", gameId, boardId, boardCardUpdates);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const createBoardCards = async (
  connection: signalR.HubConnection,
  gameId: number,
  cardIds: Array<number>
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("CreateBoardCards", gameId, cardIds);
    console.log("CreateBoardCards:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const chooseBoardCard = async (
  connection: signalR.HubConnection,
  gameId: number,
  boardId: number,
  boardCardId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("ChooseBoardCard", gameId, boardId, boardCardId);
    console.log("ChooseBoardCard:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const startGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("StartGame", gameId);
    console.log("Game started:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};
