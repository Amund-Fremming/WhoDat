import { ReactNode, createContext, useContext } from "react";
import { useState } from "react";

interface IAuthContext {
  token: string;
  setToken: React.Dispatch<React.SetStateAction<string>>;
  playerID: string;
  setPlayerID: React.Dispatch<React.SetStateAction<string>>;
  username: string;
  setUsername: React.Dispatch<React.SetStateAction<string>>;
}

const defaultContextValue: IAuthContext = {
  token: "",
  setToken: () => {},
  playerID: "",
  setPlayerID: () => {},
  username: "",
  setUsername: () => {},
};

const AuthContext = createContext<IAuthContext>(defaultContextValue);

export const useAuthProvider = () => useContext(AuthContext);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string>("");
  const [playerID, setPlayerID] = useState<string>("");
  const [username, setUsername] = useState<string>("");

  const value = {
    token,
    setToken,
    playerID,
    setPlayerID,
    username,
    setUsername,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
