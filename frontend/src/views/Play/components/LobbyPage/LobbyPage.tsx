import { View, Text, Pressable } from "react-native";
import styles from "./LobbyPageStyles";
import { PlayPages } from "../../PlayPages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/shared/assets/constants/Colors";

interface LobbyPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function LobbyPage({ setPage }: LobbyPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.id}>ID: 54234</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.headerWrapper}>
        <Text style={styles.header}>Username</Text>
        <Text style={styles.header2}>is choosing warriors</Text>
      </View>
    </View>
  );
}
