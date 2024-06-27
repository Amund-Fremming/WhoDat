import { Text, View } from "react-native";
import styles from "./StrokedTextStyles";

interface StokedTextProps {
  text: string;
}

export default function StrokedText({ text }: StokedTextProps) {
  return (
    <View style={styles.container}>
      <Text style={[styles.text, styles.shadowRight]}>{text}</Text>
      <Text style={[styles.text, styles.shadowLeft]}>{text}</Text>
      <Text style={[styles.text, styles.shadowTop]}>{text}</Text>
      <Text style={[styles.text, styles.shadowBottom]}>{text}</Text>
      <Text style={styles.text}>{text}</Text>
    </View>
  );
}
