import { Text, Pressable } from "react-native";
import styles from "./MediumButtonStyles";
import { Colors } from "@/constants/Colors";

interface MediumButtonProps {
  text: string;
  color: string;
  inverted: boolean;
  onButtonPress: () => void;
}

export default function BigButton({
  text,
  color,
  inverted,
  onButtonPress,
}: MediumButtonProps) {
  const getStyles = () => {
    if (inverted) {
      return {
        ...styles.button,
        backgroundColor: Colors.Cream,
        borderColor: color,
      };
    } else {
      return {
        ...styles.button,
        backgroundColor: color,
        borderColor: color,
      };
    }
  };

  return (
    <Pressable onPress={onButtonPress} style={getStyles()}>
      <Text style={{ ...styles.text, color: inverted ? color : Colors.Cream }}>
        {text}
      </Text>
    </Pressable>
  );
}
