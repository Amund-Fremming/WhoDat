import { ReactNode, createContext, useContext } from 'react';
import { useState } from 'react';
import { ICardDto } from '../types/CardTypes';

interface IPreloadContext {
  allGalleryCards: ICardDto[];
  setAllGalleryCards: React.Dispatch<React.SetStateAction<ICardDto[]>>;
}

const defaultContextValue: IPreloadContext = {
  allGalleryCards: [],
  setAllGalleryCards: () => {},
};

const PreloadContext = createContext<IPreloadContext>(defaultContextValue);

export const usePreloadProvider = () => useContext(PreloadContext);

export const PreloadProvider = ({ children }: { children: ReactNode }) => {
  const [allGalleryCards, setAllGalleryCards] = useState<ICardDto[]>([]);

  const value = {
    allGalleryCards,
    setAllGalleryCards,
  };

  return (
    <PreloadContext.Provider value={value}>{children}</PreloadContext.Provider>
  );
};
