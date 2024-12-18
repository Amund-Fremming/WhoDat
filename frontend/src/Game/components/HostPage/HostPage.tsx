import { View, Text, Pressable } from "react-native";
import { styles } from "./HostPageStyles";
import { PlayPages } from "../../GamePages";
import IconButton from "@/src/Shared/components/IconButton/IconButton";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { GameState } from "../../types/GameTypes";
import { HubConnection } from "@microsoft/signalr";

interface HostPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  setGameState: React.Dispatch<React.SetStateAction<GameState>>;
  handleCreateGame: (gameState: GameState) => Promise<void>;
}

export default function HostPage({ setPage, setGameState, handleCreateGame }: HostPageProps) {
  const handleBothChoosing = async () => {
    setPage(PlayPages.WAITING_PAGE);
    setGameState(GameState.BOTH_CHOSING_CARDS);
    await handleCreateGame(GameState.BOTH_CHOSING_CARDS);
  };

  const handleHostChoosing = async () => {
    setPage(PlayPages.WAITING_PAGE);
    setGameState(GameState.ONLY_HOST_CHOSING_CARDS);
    await handleCreateGame(GameState.ONLY_HOST_CHOSING_CARDS);
  };

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.textWrapper}>
        <Text style={styles.header}>Choose</Text>
        <Text style={styles.header}>the cards</Text>
      </View>
      <IconButton
        text="Your cards"
        icon="user"
        onButtonPress={handleHostChoosing}
      />
      <IconButton
        text="Split 50/50"
        icon="users"
        onButtonPress={handleBothChoosing}
      />
    </View>
  );
}
