import * as signalR from "@microsoft/signalr";

import { HUB_ENDPOINT } from "@/src/Shared/domain/URL_PATHS";
import Result from "../Shared/domain/Result";

// Create a connection to the hub
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

//move
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

export const x = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("x", gameId);
    console.log("x:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};

export const updateGameState = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<Result<boolean>> => {
  try {
    await connection.invoke("x", gameId);
    console.log("x:", gameId);
    return Result.ok(true);
  } catch (error) {
    return Result.failure("Falied to connect, check your wifi");
  }
};
