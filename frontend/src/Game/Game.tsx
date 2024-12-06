import { useState } from "react";
import { PlayPages } from "./GamePages";
import MainPage from "./components/MainPage/MainPage";
import JoinPage from "./components/JoinPage/JoinPage";
import HostPage from "./components/HostPage/HostPage";
import BoardPage from "./components/BoardPage/BoardPage";
import LobbyPage from "./components/LobbyPage/LobbyPage";
import WaitingPage from "./components/WaitingPage/WaitingPage";
import {
  createConnection,
  startConnection,
  joinGame,
  leaveGame,
  startGame,
} from "@/src/Game/GameHubClient";
import { HubConnection } from "@microsoft/signalr";
import { GameState } from "./types/GameTypes";
import { useAuthProvider } from "../Shared/state/AuthProvider";
import ErrorModal from "../Shared/components/ErrorModal/ErrorModal";

export default function Game() {
  const [page, setPage] = useState<PlayPages>(PlayPages.MAIN_PAGE);
  const [connection, setConnection] = useState<HubConnection>();
  const [gameId, setGameId] = useState<number>(0);
  const { token, playerID } = useAuthProvider();
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  //return () => {
  //if (connection) stopConnection(connection);
  //}

  const connectToHub = async () => {
    const connection = createConnection();
    setConnection(connection);

    await startConnection(connection);

    connection.on("RECEIVE_STATE", (state: string) => {
      console.log("Game state updated:", state);
      // set page and render according to return types from backend
    });

    connection.on("RECEIVE_MESSAGE", (state: string) => {
      console.log("Message received:", state);
      // set message and pass it down
    });

    connection.on("RECEIVE_PLAYERS_LEFT", (state: string) => {
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

  const handleCreateGame = async () => {
    // TODO
  };

  if (errorModalVisible)
    return (
      <>
        <ErrorModal
          errorModalVisible={errorModalVisible}
          setErrorModalVisible={setErrorModalVisible}
          message={errorMessage}
        />
      </>
    );

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
