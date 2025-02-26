import { useEffect, useRef, useState } from 'react';
import { PlayPages } from './types/GamePages';
import MainPage from './components/MainPage/MainPage';
import JoinPage from './components/JoinPage/JoinPage';
import HostPage from './components/HostPage/HostPage';
import ChooseBoardPage from './components/ChooseBoardPage/ChooseBoardPage';
import WaitingPage from './components/WaitingPage/WaitingPage';
import {
  createConnection,
  startConnection,
  startGame,
  stopConnection,
} from '@/src/Game/GameHubClient';
import { GameState } from './types/GameTypes';
import { useAuthProvider } from '../Shared/providers/AuthProvider';
import ChooseCardPage from './components/ChooseCardPage/ChooseCardPage';
import { useInfoModalProvider } from '../Shared/providers/InfoModalProvider';
import { useGameProvider } from '../Shared/providers/GameProvider';
import Gameplay from './components/Gameplay/Gameplay';
import { useGameplayProvider } from '../Shared/providers/GameplayProvider';
import { AskState } from '../Shared/types/AskState';

export default function GameRouter() {
  const [cardsToChoose, setCardsToChoose] = useState<number>(40);
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { setAskModalVisible, setQuestionReceived, setAskState } =
    useGameplayProvider();
  const {
    connection,
    setConnection,
    gameId,
    page,
    setIsHost,
    setGameState,
    setPage,
    isHost,
    setWaitingMessage,
    setOponentCardsLeft,
  } = useGameProvider();

  const isHostRef = useRef(isHost);
  const gameIdRef = useRef(gameId);

  useEffect(() => {
    connectToHub();
    return () => {
      if (connection) stopConnection(connection);
      setIsHost(false);
    };
  }, []);

  useEffect(() => {
    isHostRef.current = isHost;
    gameIdRef.current = gameId;
  }, [isHost, gameId]);

  const connectToHub = async () => {
    const con = createConnection(token);
    setConnection(con);
    await startConnection(con);

    con.on('RECEIVE_STATE', (state: GameState) => {
      setGameState(state);
      switch (state) {
        case GameState.PLAYER_LEFT: {
          toggleInfoModal(true, 'The other player left the game.');
          break;
        }
        case GameState.ONLY_HOST_CHOSING_CARDS: {
          setCardsToChoose(20);
          setWaitingMessage('Host is choosing cards');
          setPage(
            isHostRef.current
              ? PlayPages.CHOOSE_BOARD_PAGE
              : PlayPages.WAITING_PAGE
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
            setWaitingMessage('Oponent is choosing their cards');
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
        case GameState.P1_CHOOSING: {
          if (!isHostRef.current) {
            setWaitingMessage('Oponent is choosing their cards');
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
        case GameState.P1_PICKING_PLAYER: {
          if (!isHostRef.current) {
            setWaitingMessage('Oponent is choosing their warrior');
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
        case GameState.P2_PICKING_PLAYER: {
          if (isHostRef.current) {
            setWaitingMessage('Oponent is choosing their warrior');
            setPage(PlayPages.WAITING_PAGE);
          }
          break;
        }
        case GameState.BOTH_PICKED_PLAYERS: {
          setWaitingMessage('Get ready!');
          setPage(PlayPages.WAITING_PAGE);
          if (isHostRef.current) {
            setTimeout(async () => {
              await startGame(con, gameIdRef.current);
            }, 700);
          }
          break;
        }

        // Gameplay states
        case GameState.P1_TURN_STARTED: {
          setAskState(AskState.Asking);
          setPage(PlayPages.GAMEPLAY);
          break;
        }
        case GameState.P2_TURN_STARTED: {
          setAskState(AskState.Asking);
          break;
        }
        case GameState.P1_WAITING_ASK_REPLY: {
          setAskModalVisible(true);
          setAskState(
            isHostRef.current ? AskState.Waiting : AskState.Answering
          );
          break;
        }
        case GameState.P2_WAITING_ASK_REPLY: {
          setAskModalVisible(true);
          setAskState(
            isHostRef.current ? AskState.Answering : AskState.Waiting
          );
          break;
        }
        case GameState.P2_ASK_REPLIED: {
          setAskModalVisible(true);
          setAskState(
            isHostRef.current ? AskState.Answered : AskState.Finished
          );
          break;
        }
        case GameState.P1_ASK_REPLIED: {
          setAskModalVisible(true);
          setAskState(
            isHostRef.current ? AskState.Finished : AskState.Answered
          );
          break;
        }
      }
    });

    con.on('RECEIVE_MESSAGE', (message: string) =>
      setQuestionReceived(message)
    );

    con.on('RECEIVE_PLAYERS_LEFT', (num: number) => {
      setOponentCardsLeft(num);
    });

    con.on('RECEIVE_ERROR', (message: string) => {
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
    case PlayPages.WAITING_PAGE:
      return <WaitingPage />;
    case PlayPages.GAMEPLAY:
      return <Gameplay />;
  }
}
