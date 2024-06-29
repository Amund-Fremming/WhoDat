import { View, Text, Pressable } from "react-native";
import styles from "./WaitingPageStyles";
import { PlayPages } from "../../PlayPages";
import { Colors } from "@/constants/Colors";
import { Ionicons } from "@expo/vector-icons";

interface WaitingPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function WaitingPage({ setPage }: WaitingPageProps) {
  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View>
        <Text style={styles.header}>Waiting</Text>
        <Text style={styles.header}>for bro...</Text>
      </View>
    </View>
  );
}
