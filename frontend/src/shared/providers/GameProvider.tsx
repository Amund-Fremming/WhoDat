import { PlayPages } from '@/src/Game/types/GamePages';
import { GameState } from '@/src/Game/types/GameTypes';
import React, { createContext, ReactNode, useContext, useState } from 'react';

interface IGameContext {
  // hva skal den ha
  page: PlayPages;
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  gameState: GameState;
  setGameState: React.Dispatch<React.SetStateAction<GameState>>;
  gameId: number;
  setGameId: React.Dispatch<React.SetStateAction<number>>;
}

const defaultContextValue: IGameContext = {
  page: PlayPages.MAIN_PAGE,
  setPage: () => {},
  gameState: GameState.BOTH_CHOSING_CARDS,
  setGameState: () => {},
  gameId: 0,
  setGameId: () => {},
};

const GameContext = createContext<IGameContext>(defaultContextValue);

export const useInfoModalProvider = () => useContext(GameContext);

interface GameProviderProps {
  children: ReactNode;
}

export const GameProvider = ({ children }: GameProviderProps) => {
  // States

  const value = {
    // pass in all accessible states
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
