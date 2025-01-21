import React, { createContext, ReactNode, useContext, useState } from 'react';
import InfoModal from '../components/InfoModal/InfoModal';

interface IInfoModalContext {
  toggleInfoModal: (isError: boolean, message: string) => void;
}

const defaultContextValue: IInfoModalContext = {
  toggleInfoModal: () => {},
};

const InfoModalContext = createContext<IInfoModalContext>(defaultContextValue);

export const useInfoModalProvider = () => useContext(InfoModalContext);

interface InfoModalProviderProps {
  children: ReactNode;
}

export const InfoModalProvider = ({ children }: InfoModalProviderProps) => {
  const [isError, setIsError] = useState<boolean>(false);
  const [message, setMessage] = useState<string>('');
  const [modalVisible, setModalVisible] = useState<boolean>(false);

  const toggleInfoModal = (localIsError: boolean, localMesssage: string) => {
    setMessage(localMesssage);
    setIsError(localIsError);
    setModalVisible(!modalVisible);
  };

  const value = {
    toggleInfoModal,
  };

  return (
    <InfoModalContext.Provider value={value}>
      {children}
      <InfoModal
        message={message}
        isError={isError}
        modalVisible={modalVisible}
        setModalVisible={setModalVisible}
      />
    </InfoModalContext.Provider>
  );
};
