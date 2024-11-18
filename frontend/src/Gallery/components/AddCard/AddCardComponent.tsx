import { View, Text, Pressable } from "react-native";
import { styles } from "./AddCardComponentStyles";

interface AddCardComponentProps {
  onAddCardPress: () => void;
}

export function AddCardComponent({ onAddCardPress }: AddCardComponentProps) {
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
