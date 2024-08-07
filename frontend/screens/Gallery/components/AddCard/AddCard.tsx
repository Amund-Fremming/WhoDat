import { View, Text, Pressable } from "react-native";
import { styles } from "./AddCardStyles";

interface AddCardProps {
  onAddCardPress: () => void;
}

export function AddCard({ onAddCardPress }: AddCardProps) {
  return (
    <Pressable style={styles.container} onPress={onAddCardPress}>
      <View style={styles.card}>
        <View style={styles.innerCard}>
          <Text style={styles.text}>Add</Text>
          <Text style={styles.text}>Card</Text>
        </View>
      </View>
    </Pressable>
  );
}
