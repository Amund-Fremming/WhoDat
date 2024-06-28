import { View, Text } from "react-native";
import styles from "./LobbyPageStyles";
import { PlayPages } from "../../PlayPages";

interface LobbyPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function LobbyPage({ setPage }: LobbyPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>LobbyPage</Text>
    </View>
  );
}
