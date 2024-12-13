import { useEffect, useState } from "react";
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
  stopConnection,
} from "@/src/Game/GameHubClient";
import { HubConnection } from "@microsoft/signalr";
import { GameState } from "./types/GameTypes";
import { useAuthProvider } from "../Shared/state/AuthProvider";
import ErrorModal from "../Shared/components/ErrorModal/ErrorModal";

export default function Game() {
  const [page, setPage] = useState<PlayPages>(PlayPages.MAIN_PAGE);
  const [gameState, setGameState] = useState<GameState>(
    GameState.BOTH_CHOSING_CARDS
  );
  const [message, setMessage] = useState<string>("");
  const [oponentCardsLeft, setOponentCardsLeft] = useState<number>(20);
  const [connection, setConnection] = useState<HubConnection>();
  const { token, playerID } = useAuthProvider();
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

  useEffect(() => {
    connectToHub();
    return () => {
      if (connection) stopConnection(connection);
    };
  }, []);

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  const connectToHub = async () => {
    const con = createConnection();
    setConnection(con);
    await startConnection(con);

    con.on("RECEIVE_STATE", (state: GameState) => {
      setGameState(state);
      // set page and render according to return types from backend
    });

    con.on("RECEIVE_MESSAGE", (message: string) => {
      setMessage(message);
      // Display the message
    });

    con.on("RECEIVE_PLAYERS_LEFT", (num: number) => {
      setOponentCardsLeft(num);
    });
  };

  if (errorModalVisible)
    return (
      <ErrorModal
        errorModalVisible={errorModalVisible}
        setErrorModalVisible={setErrorModalVisible}
        message={errorMessage}
      />
    );

  switch (page) {
    case PlayPages.MAIN_PAGE:
      return <MainPage setPage={setPage} />;
    case PlayPages.JOIN_PAGE:
      return <JoinPage setPage={setPage} />;
    case PlayPages.HOST_PAGE:
      return <HostPage setPage={setPage} />;
    case PlayPages.BOARD_PAGE:
      return (
        <BoardPage setPage={setPage} oponentCardsLeft={oponentCardsLeft} />
      );
    case PlayPages.LOBBY_PAGE:
      return <LobbyPage setPage={setPage} />;
    case PlayPages.WAITING_PAGE:
      return <WaitingPage setPage={setPage} />;
  }
}
