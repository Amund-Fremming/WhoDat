import { useEffect, useRef, useState } from 'react';
import { PlayPages } from './types/GamePages';
import MainPage from './components/MainPage/MainPage';
import JoinPage from './components/JoinPage/JoinPage';
import HostPage from './components/HostPage/HostPage';
import ChooseBoardPage from './components/ChooseBoardPage/ChooseBoardPage';
import LobbyPage from './components/LobbyPage/LobbyPage';
import WaitingPage from './components/WaitingPage/WaitingPage';
import {
  createConnection,
  startConnection,
  stopConnection,
  updateGameState,
} from '@/src/Game/GameHubClient';
import { GameState } from './types/GameTypes';
import { useAuthProvider } from '../Shared/providers/AuthProvider';
import ChooseCardPage from './components/ChooseCardPage/ChooseCardPage';
import { useInfoModalProvider } from '../Shared/providers/InfoModalProvider';
import { useGameProvider } from '../Shared/providers/GameProvider';

export default function GameRouter() {
  const [message, setMessage] = useState<string>('');
  const [oponentCardsLeft, setOponentCardsLeft] = useState<number>(20);
  const [cardsToChoose, setCardsToChoose] = useState<number>(40);
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const {
    connection,
    setConnection,
    gameState,
    page,
    setIsHost,
    setGameState,
    setPage,
    isHost,
  } = useGameProvider();

  const isHostRef = useRef(isHost);

  useEffect(() => {
    connectToHub();
    return () => {
      if (connection) stopConnection(connection);
      setIsHost(false);
    };
  }, []);

  useEffect(() => {
    isHostRef.current = isHost;
  }, [isHost]);

  useEffect(() => {
    if (connection) updateGameState(connection, gameState);
  }, [gameState]);

  const connectToHub = async () => {
    const con = createConnection(token);
    setConnection(con);
    await startConnection(con);

    con.on('RECEIVE_STATE', (state: GameState) => {
      setGameState(state);
      switch (state) {
        case GameState.PLAYER_LEFT: {
          toggleInfoModal(false, 'The other player left the game.');
          break;
        }
        case GameState.ONLY_HOST_CHOSING_CARDS: {
          setCardsToChoose(20);
          setPage(
            isHostRef.current
              ? PlayPages.CHOOSE_BOARD_PAGE
              : PlayPages.LOBBY_PAGE
          );
          break;
        }
        case GameState.BOTH_CHOSING_CARDS: {
          setCardsToChoose(10);
          setPage(PlayPages.CHOOSE_BOARD_PAGE);
          break;
        }
        case GameState.BOTH_PICKING_PLAYER: {
          setPage(PlayPages.CHOOSE_CARD_PAGE);
          break;
        }
        case GameState.P2_CHOOSING: {
          if (isHostRef.current) {
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
        case GameState.P1_CHOOSING: {
          if (!isHostRef.current) {
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
      }
    });

    con.on('RECEIVE_MESSAGE', (message: string) => {
      setMessage(message);
      // TODO: Display the message
    });

    con.on('RECEIVE_PLAYERS_LEFT', (num: number) => {
      setOponentCardsLeft(num);
    });

    con.on('RECEIVE_ERROR', (message: string) => {
      console.log('Error msg ' + message);
      toggleInfoModal(true, message);
    });
  };

  switch (page) {
    case PlayPages.MAIN_PAGE:
      return <MainPage />;
    case PlayPages.JOIN_PAGE:
      return <JoinPage />;
    case PlayPages.HOST_PAGE:
      return <HostPage />;
    case PlayPages.CHOOSE_BOARD_PAGE:
      return <ChooseBoardPage cardsToChoose={cardsToChoose} />;
    case PlayPages.CHOOSE_CARD_PAGE:
      return <ChooseCardPage />;
    case PlayPages.LOBBY_PAGE:
      return <LobbyPage />;
    case PlayPages.WAITING_PAGE:
      return <WaitingPage />;
  }
}
