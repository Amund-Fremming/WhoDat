import { ReactNode, createContext, useContext } from 'react';
import { useState } from 'react';
import { Animated } from 'react-native';

interface ITabBarContext {
  displayTabBar:
    | Animated.Value
    | Animated.AnimatedInterpolation<string | number>
    | 'flex'
    | 'none'
    | undefined;
  setDisplayTabBar: React.Dispatch<
    React.SetStateAction<
      | Animated.Value
      | Animated.AnimatedInterpolation<string | number>
      | 'flex'
      | 'none'
      | undefined
    >
  >;
}

const defaultContextValue: ITabBarContext = {
  displayTabBar: 'flex',
  setDisplayTabBar: () => {},
};

const TabBarContext = createContext<ITabBarContext>(defaultContextValue);

export const useTabBarProvider = () => useContext(TabBarContext);

export const TabBarProvider = ({ children }: { children: ReactNode }) => {
  const [displayTabBar, setDisplayTabBar] = useState<
    | Animated.Value
    | Animated.AnimatedInterpolation<string | number>
    | 'flex'
    | 'none'
    | undefined
  >('flex');

  const value = {
    displayTabBar,
    setDisplayTabBar,
  };

  return (
    <TabBarContext.Provider value={value}>{children}</TabBarContext.Provider>
  );
};
