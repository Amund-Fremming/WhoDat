import * as signalR from "@microsoft/signalr";

import { HUB_ENDPOINT } from "@/src/Shared/domain/URL_PATHS";

// Create a connection to the hub
export const createConnection = (): signalR.HubConnection => {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${HUB_ENDPOINT}`)
    .configureLogging(signalR.LogLevel.Information)
    .build();
};

// Start the connection
export const startConnection = async (
  connection: signalR.HubConnection
): Promise<void> => {
  try {
    await connection.start();
    console.log("Connection started");
  } catch (error) {
    console.error("GameHub Error: ", error);
  }
};

// Stop the connection
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

// Define methods to call backend hub methods
export const joinGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<void> => {
  try {
    await connection.invoke("JoinGame", gameId);
    console.log("Joined game:", gameId);
  } catch (error) {
    console.error("Error joining game:", error);
  }
};

export const leaveGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<void> => {
  try {
    await connection.invoke("LeaveGame", gameId);
    console.log("Left game:", gameId);
  } catch (error) {
    console.error("Error leaving game:", error);
  }
};

export const startGame = async (
  connection: signalR.HubConnection,
  gameId: number
): Promise<void> => {
  try {
    await connection.invoke("StartGame", gameId);
    console.log("Game started:", gameId);
  } catch (error) {
    console.error("Error starting game:", error);
  }
};

// Register listeners for server-sent events
export const registerEventListeners = (
  connection: signalR.HubConnection
): void => {
  connection.on("ReceiveMessage", (message: string) => {
    console.log("Message from hub:", message);
  });

  connection.on("UpdateGameState", (state: any) => {
    console.log("Game state updated:", state);
  });
};
