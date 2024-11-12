import { useEffect, useState } from "react";
import { PlayPages } from "./PlayPages";
import MainPage from "./components/MainPage/MainPage";
import JoinPage from "./components/JoinPage/JoinPage";
import HostPage from "./components/HostPage/HostPage";
import BoardPage from "./components/BoardPage/BoardPage";
import LobbyPage from "./components/LobbyPage/LobbyPage";
import WaitingPage from "./components/WaitingPage/WaitingPage";
import {
  createConnection,
  startConnection,
  registerEventListeners,
  stopConnection,
  joinGame,
  leaveGame,
  startGame,
} from "@/util/GameHub";
import { HubConnection } from "@microsoft/signalr";
import { createGame } from "@/api/GameApi";
import { IGame, State } from "@/interfaces/IGameTypes";
import { useAuthProvider } from "@/providers/AuthProvider";

export default function Play() {
  const [page, setPage] = useState<PlayPages>(PlayPages.MAIN_PAGE);
  const [connection, setConnection] = useState<HubConnection>();
  const [gameId, setGameId] = useState<number>(0);
  const { token, playerID } = useAuthProvider();

  //return () => {
  //if (connection) stopConnection(connection);
  //}

  const connectToHub = async () => {
    const conn = createConnection();
    setConnection(conn);

    await startConnection(conn);
    registerEventListeners(conn);

    conn.on("RECEIVE_STATE", (state: string) => {
      console.log("Game state updated:", state);
      // set page and render according to return types from backend
    });

    conn.on("RECEIVE_MESSAGE", (state: string) => {
      console.log("Message received:", state);
      // set message and pass it down
    });

    conn.on("RECEIVE_PLAYERS_LEFT", (state: string) => {
      console.log("Player left updated:", state);
      // set players left and pass down
    });
  };

  const handleJoinGame = async () => {
    if (connection) {
      await joinGame(connection, gameId);
      //setGameStatus("Joined Game");
    }
  };

  const handleLeaveGame = async () => {
    if (connection) {
      await leaveGame(connection, gameId);
      //setGameStatus("Left Game");
    }
  };

  const handleStartGame = async () => {
    if (connection) {
      await startGame(connection, gameId);
      //setGameStatus("Game Started");
    }
  };

  const handleCreateGame = async (state: State) => {
    try {
      const game: IGame = {
        playerOneID: playerID,
        state: state,
      };

      const createdGameId = await createGame(game, token);
      setGameId(createdGameId);
    } catch (error) {
      console.error("Error creating game");
    }
  };

  switch (page) {
    case PlayPages.MAIN_PAGE:
      return <MainPage setPage={setPage} />;
    case PlayPages.JOIN_PAGE:
      return <JoinPage setPage={setPage} />;
    case PlayPages.HOST_PAGE:
      return <HostPage setPage={setPage} />;
    case PlayPages.BOARD_PAGE:
      return <BoardPage setPage={setPage} />;
    case PlayPages.LOBBY_PAGE:
      return <LobbyPage setPage={setPage} />;
    case PlayPages.WAITING_PAGE:
      return <WaitingPage setPage={setPage} />;
  }
}
