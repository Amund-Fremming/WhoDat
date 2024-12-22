import { View, Text, Pressable } from "react-native";
import { styles } from "./BoardPageStyles";
import { PlayPages } from "../../GamePages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";

interface BoardPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  oponentCardsLeft: number;
}

export default function BoardPage({
  setPage,
  oponentCardsLeft,
}: BoardPageProps) {
  return (
    <View style={styles.container}>
            <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <Text style={styles.header}>BoardPage</Text>
    </View>
  );
}
