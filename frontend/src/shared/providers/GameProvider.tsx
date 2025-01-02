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
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
