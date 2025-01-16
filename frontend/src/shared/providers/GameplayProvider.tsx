import React, { createContext, ReactNode, useContext, useState } from 'react';
import { AskState } from '../types/AskState';
import AskModal from '@/src/Shared/components/AskModal/AskModal';

interface IGameplayContext {
  askState: AskState;
  setAskState: React.Dispatch<React.SetStateAction<AskState>>;
  askModalVisible: boolean;
  setAskModalVisible: React.Dispatch<React.SetStateAction<boolean>>;
  questionReceived: string;
  setQuestionReceived: React.Dispatch<React.SetStateAction<string>>;
}

const defaultContextValue: IGameplayContext = {
  askState: AskState.Asking,
  setAskState: () => {},
  askModalVisible: false,
  setAskModalVisible: () => {},
  questionReceived: '',
  setQuestionReceived: () => {},
};

const GameplayContext = createContext<IGameplayContext>(defaultContextValue);

export const useGameplayProvider = () => useContext(GameplayContext);

interface GameplayProviderProps {
  children: ReactNode;
}

export const GameplayProvider = ({ children }: GameplayProviderProps) => {
  const [askState, setAskState] = useState<AskState>(AskState.Asking);
  const [askModalVisible, setAskModalVisible] = useState<boolean>(false);
  const [questionReceived, setQuestionReceived] = useState<string>('');

  const value = {
    askState,
    setAskState,
    askModalVisible,
    setAskModalVisible,
    questionReceived,
    setQuestionReceived,
  };

  return (
    <GameplayContext.Provider value={value}>
      <AskModal
        modalVisible={askModalVisible}
        setModalVisible={setAskModalVisible}
      />
      {children}
    </GameplayContext.Provider>
  );
};
