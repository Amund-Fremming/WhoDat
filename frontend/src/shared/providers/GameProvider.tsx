import { IBoard } from '@/src/Game/types/BoardTypes';
import { PlayPages } from '@/src/Game/types/GamePages';
import { GameState } from '@/src/Game/types/GameTypes';
import React, { createContext, ReactNode, useContext, useState } from 'react';

interface IGameContext {
  page: PlayPages;
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  gameState: GameState;
  setGameState: React.Dispatch<React.SetStateAction<GameState>>;
  gameId: number;
  setGameId: React.Dispatch<React.SetStateAction<number>>;
  connection?: signalR.HubConnection;
  setConnection: React.Dispatch<
    React.SetStateAction<signalR.HubConnection | undefined>
  >;
  isHost: boolean;
  setIsHost: React.Dispatch<React.SetStateAction<boolean>>;
  board: IBoard | undefined;
  setBoard: React.Dispatch<React.SetStateAction<IBoard | undefined>>;
  waitingMessage: string;
  setWaitingMessage: React.Dispatch<React.SetStateAction<string>>;
  message: string;
  setMessage: React.Dispatch<React.SetStateAction<string>>;
  oponentCardsLeft: number;
  setOponentCardsLeft: React.Dispatch<React.SetStateAction<number>>;
}

const defaultContextValue: IGameContext = {
  page: PlayPages.MAIN_PAGE,
  setPage: () => {},
  gameState: GameState.BOTH_CHOSING_CARDS,
  setGameState: () => {},
  gameId: 0,
  setGameId: () => {},
  connection: undefined,
  setConnection: () => {},
  isHost: false,
  setIsHost: () => {},
  board: undefined,
  setBoard: () => {},
  waitingMessage: '',
  setWaitingMessage: () => {},
  message: '',
  setMessage: () => {},
  oponentCardsLeft: 20,
  setOponentCardsLeft: () => {},
};

const GameContext = createContext<IGameContext>(defaultContextValue);

export const useGameProvider = () => useContext(GameContext);

interface GameProviderProps {
  children: ReactNode;
}

export const GameProvider = ({ children }: GameProviderProps) => {
  const [page, setPage] = useState<PlayPages>(PlayPages.MAIN_PAGE);
  const [gameState, setGameState] = useState<GameState>(
    GameState.BOTH_CHOSING_CARDS
  );
  const [gameId, setGameId] = useState<number>(0);
  const [connection, setConnection] = useState<signalR.HubConnection>();
  const [isHost, setIsHost] = useState<boolean>(false);
  const [board, setBoard] = useState<IBoard | undefined>(undefined);
  const [waitingMessage, setWaitingMessage] = useState<string>('');
  const [message, setMessage] = useState<string>('');
  const [oponentCardsLeft, setOponentCardsLeft] = useState<number>(20);

  const value = {
    page,
    setPage,
    gameState,
    setGameState,
    gameId,
    setGameId,
    connection,
    setConnection,
    isHost,
    setIsHost,
    board,
    setBoard,
    waitingMessage,
    setWaitingMessage,
    message,
    setMessage,
    oponentCardsLeft,
    setOponentCardsLeft,
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
