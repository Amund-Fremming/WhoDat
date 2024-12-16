import { View, Text, Pressable } from "react-native";
import { styles } from "./WaitingPageStyles";
import { PlayPages } from "../../GamePages";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { Ionicons } from "@expo/vector-icons";

interface WaitingPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  gameId: number;
}

export default function WaitingPage({ setPage, gameId }: WaitingPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.id}>ID: {gameId}</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.headerWrapper}>
        <Text style={styles.header}>Waiting</Text>
        <Text style={styles.header}>for bro...</Text>
      </View>
    </View>
  );
}
