import { useState } from "react";
import { PlayPages } from "./PlayPages";
import MainPage from "./components/MainPage/MainPage";
import JoinPage from "./components/JoinPage/JoinPage";
import HostPage from "./components/HostPage/HostPage";
import BoardPage from "./components/BoardPage/BoardPage";
import LobbyPage from "./components/LobbyPage/LobbyPage";
import WaitingPage from "./components/WaitingPage/WaitingPage";

export default function Play() {
  const [page, setPage] = useState<PlayPages>(PlayPages.MAIN_PAGE);

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
