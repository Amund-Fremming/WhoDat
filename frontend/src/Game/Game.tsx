import { useEffect, useRef, useState } from 'react';
import { View } from 'react-native';
import { PlayPages } from './types/GamePages';
import MainPage from './components/MainPage/MainPage';
import JoinPage from './components/JoinPage/JoinPage';
import HostPage from './components/HostPage/HostPage';
import ChooseBoardPage from './components/ChooseBoardPage/ChooseBoardPage';
import LobbyPage from './components/LobbyPage/LobbyPage';
import WaitingPage from './components/WaitingPage/WaitingPage';
import {
  createBoardCards,
  createConnection,
  joinGame,
  startConnection,
  stopConnection,
  subscribeToGameAsHost,
  updateGameState,
} from '@/src/Game/GameHubClient';
import { GameState } from './types/GameTypes';
import { useAuthProvider } from '../Shared/providers/AuthProvider';
import { createGame, getBoardWithBoardCards } from './GameClient';
import ChooseCardPage from './components/ChooseCardPage/ChooseCardPage';
import { useInfoModalProvider } from '../Shared/providers/InfoModalProvider';

export default function Game() {
  const [message, setMessage] = useState<string>('');
  const [gameId, setGameId] = useState<number>(0);
  const [oponentCardsLeft, setOponentCardsLeft] = useState<number>(20);
  const [connection, setConnection] = useState<signalR.HubConnection>();
  const [isHost, setIsHost] = useState<boolean>(false);
  const [cardsToChoose, setCardsToChoose] = useState<number>(40);

  const isHostRef = useRef(isHost);
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();

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
    // kanskje buggy
    if (connection) updateGameState(connection, gameState);
  }, [gameState]);

  const handleCreateGame = async (gameState: GameState) => {
    var result = await createGame(gameState, token);
    if (result.isError) {
      handleError(result.message, true);
      setPage(PlayPages.MAIN_PAGE);
      return;
    }

    if (result.data && connection) {
      setGameId(result.data);
      setIsHost(true);
      await subscribeToGameAsHost(connection, result.data);
    } else {
      handleError('Failed to set incomming game id. Connection failed.', true);
    }
  };

  const handleCreateBoardcards = async (cardIds: number[]) => {
    if (connection) {
      var result = await createBoardCards(connection, gameId, cardIds);
      if (result.isError) {
        handleError(result.message, false);
      }
    }
  };

  const handleJoinGame = async () => {
    if (connection) {
      await joinGame(connection, gameId);
      setIsHost(false);
    } else handleError('Connection was broken.', true);
  };

  const fetchBoard = async () => {
    var result = await getBoardWithBoardCards(gameId, token);
    if (result.isError) {
      handleError(result.message, true);
    }
  };

  const connectToHub = async () => {
    const con = createConnection(token);
    setConnection(con);
    await startConnection(con);

    con.on('RECEIVE_STATE', (state: GameState) => {
      console.log('State ' + state);

      setGameState(state);
      switch (state) {
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
      return <MainPage setPage={setPage} />;
    case PlayPages.JOIN_PAGE:
      return (
        <JoinPage
          handleJoinGame={handleJoinGame}
          setGameId={setGameId}
          setPage={setPage}
        />
      );
    case PlayPages.HOST_PAGE:
      return (
        <HostPage
          handleCreateGame={handleCreateGame}
          setGameState={setGameState}
          setPage={setPage}
        />
      );
    case PlayPages.CHOOSE_BOARD_PAGE:
      return (
        <ChooseBoardPage
          cardsToChoose={cardsToChoose}
          setPage={setPage}
          handleCreateBoardcards={handleCreateBoardcards}
        />
      );
    case PlayPages.CHOOSE_CARD_PAGE:
      return <ChooseCardPage setPage={setPage} fetchBoard={fetchBoard} />;
    case PlayPages.LOBBY_PAGE:
      return <LobbyPage setPage={setPage} />;
    case PlayPages.WAITING_PAGE:
      return <WaitingPage gameId={gameId} setPage={setPage} />;
  }
}
